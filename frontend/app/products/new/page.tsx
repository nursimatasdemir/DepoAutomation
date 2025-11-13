'use client';

import { useState, useEffect } from 'react';
import {useRouter} from 'next/navigation';
import axiosInstance from '../../../utils/axiosInstance';
import { CreateProductRequest } from '@/types/product';

interface Category {
    id: string;
    name: string;
}

export default function AddProductPage() {
    const router = useRouter();

    const [formData, setFormData] = useState<CreateProductRequest>({
        sku: '',
        name: '',
        barcode: '',
        categoryId: ''
    });

    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string>('');

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const response = await axiosInstance.get('/categories');
                setCategories(response.data);
                // İlk kategoriyi varsayılan seç (Opsiyonel)
                if (response.data.length > 0) {
                    setFormData(prev => ({...prev, categoryId: response.data[0].id}));
                }
            } catch (err) {
                console.error("Kategoriler yüklenemedi", err);
                setError("Kategoriler yüklenirken hata oluştu.");
            }
        };
        fetchCategories();
    }, []);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const {name, value} = e.target;
        setFormData(prev => ({...prev, [name]: value}));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError('');

        try {
            await axiosInstance.post('/Products', formData);

            alert('Ürün başarıyla eklendi!');
            router.push('/');
        } catch (err: any) {
            console.error(err);
            if (err.response?.data?.errors) {
                const messages = Object.values(err.response.data.errors).flat().join(', ');
                setError(messages);
            } else {
                setError('ürün eklenirken hata oluştu!');
            }
        } finally {
            setLoading(false);
        }
    };
    return (
        <div className="min-h-screen bg-gray-100 py-10 px-4 sm:px-6 lg:px-8">
            <div className="max-w-md mx-auto bg-white rounded shadow p-6">
                <h2 className="text-2xl font-bold mb-6 text-gray-900">Yeni Ürün Ekle</h2>

                {error && (
                    <div className="mb-4 p-3 bg-red-100 text-red-700 border border-red-400 rounded text-sm">
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit} className="space-y-4">

                    {/* SKU */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">SKU (Stok Kodu)</label>
                        <input
                            name="sku"
                            value={formData.sku}
                            onChange={handleChange}
                            required
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                            placeholder="Örn: ELMA-001"
                        />
                    </div>

                    {/* Ürün Adı */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Ürün Adı</label>
                        <input
                            name="name"
                            value={formData.name}
                            onChange={handleChange}
                            required
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                            placeholder="Örn: Kırmızı Elma"
                        />
                    </div>

                    {/* Barkod */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Barkod</label>
                        <input
                            name="barcode"
                            value={formData.barcode}
                            onChange={handleChange}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                            placeholder="869..."
                        />
                    </div>

                    {/* Kategori Seçimi */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Kategori</label>
                        <select
                            name="categoryId"
                            value={formData.categoryId}
                            onChange={handleChange}
                            required
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                        >
                            <option value="">Seçiniz...</option>
                            {categories.map(cat => (
                                <option key={cat.id} value={cat.id}>{cat.name}</option>
                            ))}
                        </select>
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
                            className={`px-4 py-2 text-sm font-medium text-white bg-indigo-600 rounded-md hover:bg-indigo-700 ${loading ? 'opacity-50' : ''}`}
                        >
                            {loading ? 'Kaydediliyor...' : 'Kaydet'}
                        </button>
                    </div>

                </form>
            </div>
        </div>
    );
}