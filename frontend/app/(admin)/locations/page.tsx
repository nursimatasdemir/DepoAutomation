'use client'

import React, {useEffect, useState} from 'react'
import {useRouter} from 'next/navigation';

import locationService from '@/services/locationService';
import {Location} from '@/types/location';

export default function LocationListPage() {
    const router = useRouter();
    const [locations, setLocations] = useState<Location[]>([]);
    
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    
    useEffect(() => {
        fetchLocation();
    }, []);
    
    const fetchLocation = async () => {
        setLoading(true);
        try {
            const location = await locationService.getAll();
            setLocations(location);
        } catch (err) {
            console.error("Lokasyon bilgileri alınamadı", err);
            setError("Lokasyon bilgilerinin bulunduğu liste yüklenirken bir hata oluştu.");
        } finally {
            setLoading(false);
        }
    };
    
    const handleDelete = async (locationId: string) => {
        if(!window.confirm("Bu lokasyon bilgisini silmek istediğinize emin misiniz?"))
        {
            return;
        }
        
        try {
            await locationService.delete(locationId);
            setLocations(prev => prev.filter(l => l.id !== locationId));
            alert("Lokasyon bilgisi başarıyla silindi!");
        } catch (err: any) {
            console.error("Lokasyon bilgisi silinemedi: ", err);
            if(err.response && err.response.status === 403) {
                setError("Bu işlem için 'Admin' yetkisine sahip olmalısınız!");
            } else {
                setError("Bir hata oluştu");
            }
        }
    } ;
    
    if (loading) return <div className="p-10 text-center">Yükleniyor...</div>;

    return (
        <div className="px-4 py-6 sm:px-0">
            <div className="flex justify-between items-center mb-6">
                <h2 className="text-2xl font-bold text-gray-800">Lokasyon Bilgileri</h2>
                <button
                    onClick={() => router.push('/locations/new')}
                    className="bg-indigo-600 text-white px-4 py-2 rounded hover:bg-indigo-700"
                >
                    + Yeni Lokasyon
                </button>
            </div>

            {error && (
                <div className="bg-red-100 border-l-4 border-red-500 text-red-700 p-4 mb-4">
                    {error}
                </div>
            )}

            <div className="bg-white shadow overflow-hidden sm:rounded-lg">
                <table className="min-w-full divide-y divide-gray-200">
                    <thead className="bg-gray-50">
                    <tr>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Lokasyon Adı</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Kodu</th>
                        <th className="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase tracking-wider">İşlemler</th>
                    </tr>
                    </thead>
                    <tbody className="bg-white divide-y divide-gray-200">
                    {locations.map((location) => (
                        <tr key={location.id}>
                            <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{location.type}</td>
                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{location.code}</td>

                            {/* İşlemler Kolonu (Ortalanmış ve Rozet Stilinde) */}
                            <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-center">
                                <button
                                    onClick={() => router.push(`/locations/${location.id}`)}
                                    className="px-3 py-1 inline-flex text-sm leading-5 font-semibold rounded-full bg-indigo-100 text-indigo-800 hover:bg-indigo-200 mr-4"
                                >
                                    Düzenle
                                </button>
                                <button
                                    onClick={() => handleDelete(location.id)}
                                    className="text-red-600 hover:text-red-900"
                                >
                                    Sil
                                </button>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
                {locations.length === 0 && !error && (
                    <div className="text-center py-10 text-gray-500">Hiç kategori bulunamadı.</div>
                )}
            </div>
        </div>
    );
    
}