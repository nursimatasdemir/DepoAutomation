'use client'

import { useState, useEffect } from 'react';
import { useRouter, useParams } from 'next/navigation';
import axiosInstance from '../../../utils/axiosInstance';
import {Product, CreateProductRequest, UpdateProductRequest} from '@/types/product';
import { Category } from '@/types/category';

export default function UpdateProductPage() {
    const router = useRouter();
    const params = useParams();
    const id = params.id as string;

    const [formData, setFormData] = useState<UpdateProductRequest>({
        Id:'',
        sku: '',
        name: '',
        barcode: '',
        categoryId: '',
    });

    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        if(!id)
        {
            return;
        }

        const fetchData = async () => {
            try {
                const [categoriesRes, productRes] = await Promise.all([
                    axiosInstance.get('/categories'),
                    axiosInstance.get(`/products/${id}`)
                ]);

                setCategories(categoriesRes.data);

                const product: Product = productRes.data;
                setFormData({
                    Id: id,
                    sku: product.sku,
                    name: product.name,
                    barcode: product.barcode,
                    categoryId: product.categoryId
                });
            } catch (err:any) {
                console.error(err);
                setError("ürün veya kategori yüklenirken bir hata oluştu!");
            } finally {
                setLoading(false);
            }
        };
        fetchData();
    }, [id]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const {name, value} = e.target;
        setFormData(prev => ({...prev, [name]: value}));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError('');

        try {
            await axiosInstance.put(`/products/${id}`, formData);

            alert("Ürün başarıyla güncellendi!");

            router.push('/');
        } catch (err:any) {
            console.error(err);
            console.error("ürün güncellenemedi: ", err);
            if(err.response && err.response.status === 403) {
                setError("Bu işlem için 'Admin' yetkisine sahip olmalısınız!");
            } else {
                setError("ürün güncellenirken hata oluştu!");
            }
        } finally {
            setLoading(false);
        }
    };

    if(loading && !formData.name)
    {
        return <div className="p-10 text-center">Ürün bilgileri yükleniyor...</div>;    }

    return (
        <div className="min-h-screen bg-gray-100 py-10 px-4 sm:px-6 lg:px-8">
            <div className="max-w-md mx-auto bg-white rounded shadow p-6">
                <h2 className="text-2xl font-bold mb-6 text-gray-900">Ürünü Düzenle</h2>

                {error && (
                    <div className="mb-4 p-3 bg-red-100 text-red-700 border border-red-400 rounded text-sm">
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700">SKU (Stok Kodu)</label>
                        <input
                            name="sku"
                            value={formData.sku}
                            onChange={handleChange}
                            required
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                        />
                    </div>

                    {/* Ürün Adı */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Ürün Adı</label>
                        <input
                            name="name"
                            value={formData.name} // State'den gelir
                            onChange={handleChange}
                            required
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                        />
                    </div>

                    {/* Barkod */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Barkod</label>
                        <input
                            name="barcode"
                            value={formData.barcode} // State'den gelir
                            onChange={handleChange}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                        />
                    </div>

                    {/* Kategori Seçimi */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Kategori</label>
                        <select
                            name="categoryId"
                            value={formData.categoryId} // State'den gelir
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
                            onClick={() => router.push('/')} // Ana Sayfaya dön
                            className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50"
                        >
                            İptal
                        </button>
                        <button
                            type="submit"
                            disabled={loading}
                            className={`px-4 py-2 text-sm font-medium text-white bg-indigo-600 rounded-md hover:bg-indigo-700 ${loading ? 'opacity-50' : ''}`}
                        >
                            {loading ? 'Güncelleniyor...' : 'Güncelle'}
                        </button>
                    </div>

                </form>
            </div>
        </div>
    );
}
