'use client'

import React, {useEffect, useState} from 'react';

import jobService from '@/services/jobService';
import {Job, JobType} from '@/types/job';

import inventoryService from '@/services/inventoryService';

export const OperatorDashboard = () => {
    const [jobs, setJobs] = useState<Job[]>([]);
    
    const [processingId, setProcessingId] = useState<string | null>(null);
    
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    
    useEffect(() => {
        fetchJobs();
    }, []);
    
    const fetchJobs = async () => {
        setLoading(true);
        try {
            const data = await jobService.getPendingJobs();
            setJobs(data);
        } catch (err) {
            console.error("İşler yüklenemedi", err);
            setError("İş listesi yüklenirken bir hata oluştu.");
        } finally {
            setLoading(false);
        }
    };
    
    const handleComplete = async (job: Job) => {
        const confirmMsg = `Bu işi (${job.items.length} kalem) ve ilgili stok hareketlerini onaylıyor musunuz?`;
        if(!window.confirm(confirmMsg)) return;
        
        setProcessingId(job.id);
        setError('');
        
        try {
            for(const item of job.items) {
                const type = job.type;
                
                if(type === 'Yerleştirme' || type === 'Transfer') {
                    if(!item.sourceLocationId || !item.targetLocationId) {
                        throw new Error(`Satır ID: ${item.id} için kaynak veya hedef lokasyon eksik!`);
                    }
                    
                    await inventoryService.transferStock({
                        productId: item.productId,
                        sourceLocationId: item.sourceLocationId,
                        destinationLocationId: item.targetLocationId,
                        quantityToTransfer: item.quantity,
                        sourceDocument: job.sourceDocument || `Job-${job.id}` 
                    });
                }
                else if (type === 'Toplama') {
                    if(!item.sourceLocationId) {
                        throw new Error(`Satır ID: ${item.id} için kaynak lokasyon eksik!`);
                    }
                    
                    await inventoryService.pickStock({
                        productId: item.productId,
                        sourceLocationId: item.sourceLocationId,
                        quantityToPick: item.quantity,
                        sourceDocument: job.sourceDocument || `Job-${job.id}`
                    });
                }
                //Burada sayım işlemi yapıcaz daha sonra
            }
            await jobService.completeJob(job.id);
            setJobs(prevJobs => prevJobs.filter(j => j.id !== job.id));
            alert("İş ve stok hareketleri başarıyla tamamlandı!");
            
        } catch (err:any) {
            console.error(err);

            const msg = err.response?.data?.detail || err.message || "İşlem sırasında bir hata oluştu.";
            alert(`HATA: ${msg} \n(İşlem yarıda kesildi, lütfen stokları kontrol edin.)`);
        } finally {
            setProcessingId(null);
        }
    };
    
    
    if(loading) return <div className="p-10 text-center">İşler yükleniyor...</div>
    

    return (
        <div>
            <h2 className="text-3xl font-bold text-gray-900 mb-6">
                Operatör İş Listesi (Bekleyenler) 
            </h2>

            {error && (
                <div className="mb-4 p-3 bg-red-100 text-red-700 rounded">
                    {error}
                </div>
            )}

            <div className="bg-white shadow overflow-hidden sm:rounded-lg">
                <table className="min-w-full divide-y divide-gray-200">
                    <thead className="bg-gray-50">
                    <tr>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">İş Tipi</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Belge No</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Tarih</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Kaç Kalem?</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">İşlem</th>
                    </tr>
                    </thead>
                    <tbody className="bg-white divide-y divide-gray-200">
                    {jobs.map((job) => (
                        <tr key={job.id}>
                            <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{job.type}</td>
                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{job.sourceDocument}</td>
                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                                {new Date(job.createdAt).toLocaleString('tr-TR')}
                            </td>
                            <td className="px-6 py-4 text-sm text-gray-500">
                                {job.items.length} Kalem Ürün
                            </td>
                            <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                                <button
                                    className={`font-bold ${processingId === job.id ? 'text-gray-400 cursor-not-allowed' : 'text-green-600 hover:text-green-900'}`}
                                    onClick={() => handleComplete(job)}
                                    disabled={processingId === job.id} // İşleniyorsa butona basılamasın
                                >
                                    {processingId === job.id ? 'Bekliyor...' : 'Tamamla ✅'}
                                </button>
                            </td>
                            <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                                <button
                                    className="px-6 py-4 inline-flex text-lg leading-5 font-semibold rounded-full bg-indigo-100 text-indigo-800 hover:bg-indigo-200 mr-4 "
                                    onClick={() => window.location.href = `/jobs/${job.id}`} // <-- YENİSİ (Router.push da olur)
                                >
                                    🔎
                                </button>
                            </td>

                        </tr>
                    ))}

                    {jobs.length === 0 && (
                        <tr>
                            <td colSpan={5} className="text-center py-10 text-gray-500">
                            Bekleyen iş bulunmamaktadır.
                            </td>
                        </tr>
                    )}
                    </tbody>
                </table>
            </div>
        </div>
    );
};