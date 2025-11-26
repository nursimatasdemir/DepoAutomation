'use client'

import React, {useState, useEffect} from 'react';
import {useRouter} from 'next/navigation';

import jobService from '@/services/jobService';
import {JobType, CreateJobItemRequest, Job} from '@/types/job'

import productService from '@/services/productService';
import {Product} from '@/types/product';

import locationService from '@/services/locationService';
import {Location} from '@/types/location';


export default function CreateJobPage() {
    const router = useRouter();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    
    const [products, setProducts] = useState<Product[]>([]);
    const [locations, setLocations] = useState<Location[]>([]);
    
    const [jobType, setJobType] = useState<JobType>(JobType.Yerleştirme);
    const [sourceDocument, setSourceDocument] = useState('');
    
    const [recentJobs, setRecentJobs] = useState<Job[]>([]);
    
    
    const [items, setItems] = useState<CreateJobItemRequest[]>([
        {productId: '', quantity: 0, sourceLocationId: '', targetLocationId: ''}
    ]);
    
    useEffect(() => {
        const fetchData = async () => {
            const role = localStorage.getItem('userRole');
            if(role !== 'Admin'){
                alert("Bu sayfaya erişim yetkiniz yok!");
                router.push("/");
            }
            
            try {
                const [prodRes, locRes, jobsRes] = await Promise.all([
                    productService.getAll(),
                    locationService.getAll(),
                    jobService.getPendingJobs()
                ]);
                setProducts(prodRes);
                setLocations(locRes);
                setRecentJobs(jobsRes);
            } catch (err) {
                console.error(err);
                setError("Ürün ve lokasyon listeleri yüklenemedi.");
            }
        };
        fetchData();
    },[]);
    
    const addItem = () => {
        setItems([...items, { productId: '', quantity: 0, sourceLocationId: '', targetLocationId: '' }]);
    };
    
    const removeItem = (index: number) => {
        const newItems = [...items];
        newItems.splice(index, 1);
        setItems(newItems);
    };
    
    const handleItemChange = (index: number, field: keyof CreateJobItemRequest, value: any) => {
        const newItems = [...items];
        //@ts-ignore
        newItems[index][field] = value;
        setItems(newItems);
    };
    
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        
        if(items.length === 0 || items.some(i => !i.productId || i.quantity<=0)) {
            setError("Lğtfen tüm satırları eksiksiz doldurun (Ürün ve Miktar zorunludur.");
            setLoading(false);
            return;
        }
        
        try {
            const cleanItems = items.map(i => ({
                ...i,
                sourceLocationId: i.sourceLocationId === '' ? undefined : i.sourceLocationId,
                targetLocationId: i.targetLocationId === '' ? undefined : i.targetLocationId,
            }));

            await jobService.createJob(
                {
                    type: Number(jobType),
                    sourceDocument,
                    items: cleanItems,
                }
            );

            alert("İş emri başarıyla oluşturuldu!");
            setSourceDocument('');
            setItems([{ productId: '', quantity: 0, sourceLocationId: '', targetLocationId: '' }]);
            const updatedJobs = await jobService.getPendingJobs();
            setRecentJobs(updatedJobs);
            
        } catch (err: any) {
            console.error(err);
            setError("İş oluşturulurken hata meydana geldi.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="max-w-7xl mx-auto py-8 px-4">
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">

                {/* --- SOL KOLON: İŞ OLUŞTURMA FORMU (2/3 Genişlik) --- */}
                <div className="lg:col-span-2">
                    <h1 className="text-2xl font-bold text-gray-900 mb-6">Yeni İş Emri Oluştur</h1>

                    {error && (
                        <div className="mb-4 p-3 bg-red-100 text-red-700 rounded border border-red-400">
                            {error}
                        </div>
                    )}

                    <form onSubmit={handleSubmit} className="space-y-6">

                        {/* Üst Bilgiler */}
                        <div className="bg-white p-6 rounded-lg shadow grid grid-cols-1 md:grid-cols-2 gap-6">
                            <div>
                                <label className="block text-sm font-medium text-gray-700">İş Tipi</label>
                                <select
                                    value={jobType}
                                    onChange={(e) => setJobType(Number(e.target.value))}
                                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm text-gray-900"
                                >
                                    <option value={JobType.Yerleştirme}>Yerleştirme (Mal Kabul -&gt; Raf)</option>
                                    <option value={JobType.Toplama}>Toplama (Raf -&gt; Sevkiyat)</option>
                                    <option value={JobType.Transfer}>Transfer (Raf -&gt; Raf)</option>
                                    <option value={JobType.Sayım}>Sayım</option>
                                </select>
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700">Kaynak Belge</label>
                                <input
                                    type="text"
                                    value={sourceDocument}
                                    onChange={(e) => setSourceDocument(e.target.value)}
                                    placeholder="Örn: Sipariş #123"
                                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm text-gray-900"
                                />
                            </div>
                        </div>

                        {/* İş Kalemleri */}
                        <div className="bg-white p-6 rounded-lg shadow">
                            <div className="flex justify-between items-center mb-4">
                                <h3 className="text-lg font-medium text-gray-900">İş Kalemleri</h3>
                                <button
                                    type="button"
                                    onClick={addItem}
                                    className="px-3 py-1 text-sm bg-indigo-50 text-indigo-700 rounded hover:bg-indigo-100"
                                >
                                    + Satır Ekle
                                </button>
                            </div>

                            {items.map((item, index) => (
                                <div key={index} className="flex flex-col md:flex-row gap-4 mb-4 p-4 bg-gray-50 rounded border border-gray-200 items-end">
                                    {/* Ürün */}
                                    <div className="flex-1">
                                        <label className="block text-xs font-medium text-gray-500 mb-1">Ürün</label>
                                        <select
                                            value={item.productId}
                                            onChange={(e) => handleItemChange(index, 'productId', e.target.value)}
                                            required
                                            className="block w-full px-2 py-1 text-sm border border-gray-300 rounded text-gray-900"
                                        >
                                            <option value="">Seçiniz...</option>
                                            {products.map(p => (
                                                <option key={p.id} value={p.id}>{p.name}</option>
                                            ))}
                                        </select>
                                    </div>

                                    {/* Kaynak */}
                                    <div className="flex-1">
                                        <label className="block text-xs font-medium text-gray-500 mb-1">Kaynak</label>
                                        <select
                                            value={item.sourceLocationId || ''}
                                            onChange={(e) => handleItemChange(index, 'sourceLocationId', e.target.value)}
                                            className="block w-full px-2 py-1 text-sm border border-gray-300 rounded text-gray-900"
                                        >
                                            <option value="">(Yok)</option>
                                            {locations.map(l => (
                                                <option key={l.id} value={l.id}>{l.code}</option>
                                            ))}
                                        </select>
                                    </div>

                                    {/* Hedef */}
                                    <div className="flex-1">
                                        <label className="block text-xs font-medium text-gray-500 mb-1">Hedef</label>
                                        <select
                                            value={item.targetLocationId || ''}
                                            onChange={(e) => handleItemChange(index, 'targetLocationId', e.target.value)}
                                            className="block w-full px-2 py-1 text-sm border border-gray-300 rounded text-gray-900"
                                        >
                                            <option value="">(Yok)</option>
                                            {locations.map(l => (
                                                <option key={l.id} value={l.id}>{l.code}</option>
                                            ))}
                                        </select>
                                    </div>

                                    {/* Miktar */}
                                    <div className="w-20">
                                        <label className="block text-xs font-medium text-gray-500 mb-1">Miktar</label>
                                        <input
                                            type="number"
                                            value={item.quantity}
                                            onChange={(e) => handleItemChange(index, 'quantity', parseFloat(e.target.value))}
                                            min="1"
                                            className="block w-full px-2 py-1 text-sm border border-gray-300 rounded text-gray-900"
                                        />
                                    </div>

                                    {/* Sil */}
                                    <button
                                        type="button"
                                        onClick={() => removeItem(index)}
                                        disabled={items.length === 1}
                                        className="text-red-500 hover:text-red-700 mb-1 disabled:opacity-30"
                                    >
                                        Sil
                                    </button>
                                </div>
                            ))}
                        </div>

                        <div className="flex justify-end">
                            <button
                                type="submit"
                                disabled={loading}
                                className="bg-green-600 text-white px-6 py-3 rounded-md hover:bg-green-700 font-medium shadow-sm disabled:opacity-50"
                            >
                                {loading ? 'Oluşturuluyor...' : 'İş Emrini Oluştur'}
                            </button>
                        </div>
                    </form>
                </div>

                {/* --- SAĞ KOLON: BEKLEYEN İŞLER LİSTESİ (1/3 Genişlik) --- */}
                <div className="lg:col-span-1">
                    <div className="bg-white rounded-lg shadow sticky top-6 overflow-hidden">
                        <div className="px-4 py-3 border-b border-gray-200 bg-gray-50">
                            <h3 className="text-lg font-medium text-gray-900">Bekleyen İşler</h3>
                        </div>
                        <ul className="divide-y divide-gray-200 max-h-[600px] overflow-y-auto">
                            {recentJobs.length === 0 ? (
                                <li className="p-4 text-sm text-gray-500 text-center">Henüz bekleyen iş yok.</li>
                            ) : (
                                recentJobs.map((job) => (
                                    <li key={job.id} className="p-4 hover:bg-gray-50">
                                        <div className="flex justify-between items-start">
                                            <div>
                                                <p className="text-sm font-bold text-indigo-600">
                                                    {job.type}
                                                </p>
                                                <p className="text-xs text-gray-500 mt-1">
                                                    Belge: <span className="font-medium text-gray-700">{job.sourceDocument || '-'}</span>
                                                </p>
                                                <p className="text-xs text-gray-400 mt-1">
                                                    {new Date(job.createdAt).toLocaleString('tr-TR')}
                                                </p>
                                            </div>
                                            <span className="inline-flex items-center px-2 py-0.5 rounded text-xs font-medium bg-yellow-100 text-yellow-800">
                                        {job.status}
                                    </span>
                                        </div>
                                        {/* İş Kalemlerinin Özeti */}
                                        <div className="mt-2 border-t border-gray-100 pt-2">
                                            {job.items.map(item => (
                                                <div key={item.id} className="text-xs text-gray-600 flex justify-between">
                                                    {/* Ürün adını şu an ID olarak gösteriyoruz, ilerde eşleştirebiliriz */}
                                                    <span className="truncate w-32" title={item.productId}>Ürün: ...{item.productId.substring(0,6)}</span>
                                                    <span className="font-semibold">{item.quantity} ad.</span>
                                                </div>
                                            ))}
                                        </div>
                                    </li>
                                ))
                            )}
                        </ul>
                    </div>
                </div>
                {/* --- SAĞ KOLON BİTTİ --- */}

            </div>
        </div>
    );
};




