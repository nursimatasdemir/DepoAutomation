'use client'

import {useState, useEffect} from 'react';
import {useRouter, useParams} from 'next/navigation';

import locationService from '@/services/locationService';
import axiosInstance from '@/utils/axiosInstance';

export default function EditLocationPage() {
    const router = useRouter();
    const params = useParams();
    const id = params.id as string;
    
    const [code, setCode] = useState('');
    const [type, setType] = useState('');
    
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState('');
    
    useEffect(() => {
        const fetchLocation = async () => {
            try {
                const locationRes = await locationService.getById(id);
                setCode(locationRes.code);
                setType(locationRes.type);
            } catch (err) {
                console.error(err);
                setError('Lokasyon bilgileri yüklenemedi.');
            } finally {
                setLoading(false);
            }
        };
        if (id) fetchLocation();
    }, [id]);
    
    const handleSubmit = async(e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        
        try {
            await locationService.update(id, {id, code, type});
            alert("Lokasyon güncellendi");
            router.push('/locations');
        } catch (err:any) {
            console.error(err);
            setError("Güncelleme sırasında bir hata oluştu.");
        } finally {
            setSaving(false);
        }
    };
    
    if(loading) return <div className = "p-10 text-center">Yükleniyor...</div>;

    return (
        <div className="max-w-md mx-auto bg-white rounded shadow p-6 mt-10">
            <h2 className="text-2xl font-bold mb-6 text-gray-900">Lokasyonu Düzenle</h2>

            {error && (
                <div className="mb-4 p-3 bg-red-100 text-red-700 border border-red-400 rounded">
                    {error}
                </div>
            )}

            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <label className="block text-sm font-medium text-gray-700">Lokasyon Kodu</label>
                    <input
                        type="text"
                        value={code}
                        onChange={(e) => setCode(e.target.value)}
                        required
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                    />
                </div>
                <div>
                    <label className="block text-sm font-medium text-gray-700">Lokasyon Adı</label>
                    <input
                        type="text"
                        value={type}
                        onChange={(e) => setType(e.target.value)}
                        required
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                    />
                </div>

                <div className="flex justify-end space-x-3 pt-4">
                    <button
                        type="button"
                        onClick={() => router.back()}
                        className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50"
                    >
                        İptal
                    </button>
                    <button
                        type="submit"
                        disabled={saving}
                        className="px-4 py-2 text-sm font-medium text-white bg-indigo-600 rounded-md hover:bg-indigo-700 disabled:opacity-50"
                    >
                        {saving ? 'Güncelleniyor...' : 'Güncelle'}
                    </button>
                </div>
            </form>
        </div>
    );


}

