'use client'

import React, {useState} from 'react';
import {useRouter} from 'next/navigation';

import locationservice from '@/services/locationService';

export default function NewLocationPage() {
    const router = useRouter();
    
    const [type, setType] = useState('');
    const [code, setCode] = useState('');
    
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    
    const handleSubmit = async (e:React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        
        try {
            await locationservice.create({code, type});
            alert('Lokasyon kaydı başarıyla oluştu!');
            router.push('/locations');
        } catch (err:any) {
            console.error(err);
            setError('Lokasyon oluşturulurken bir hata oluştu.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="max-w-md mx-auto bg-white rounded shadow p-6 mt-10">
            <h2 className="text-2xl font-bold mb-6 text-gray-900">Yeni Lokasyon Ekle</h2>

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
                        placeholder="Örn: REC-01"
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
                        placeholder="Örn: Sevkiyat Alani"
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
                        disabled={loading}
                        className="px-4 py-2 text-sm font-medium text-white bg-indigo-600 rounded-md hover:bg-indigo-700 disabled:opacity-50"
                    >
                        {loading ? 'Kaydediliyor...' : 'Kaydet'}
                    </button>
                </div>
            </form>
        </div>
    );

}