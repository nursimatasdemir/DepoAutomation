'use client'

import { useState, useEffect } from 'react';
import { useRouter, useParams } from 'next/navigation';
import axiosInstance from '@/utils/axiosInstance';
import {Product, CreateProductRequest, UpdateProductRequest} from '@/types/product';
import { Category } from '@/types/category';
import {StockTransaction} from '@/types/inventory';
import locationService from '@/services/locationService'
import {Location} from '@/types/location';

type UpdateFormData = Omit<CreateProductRequest, 'sku'>;

export default function UpdateProductPage() {
    const router = useRouter();
    const params = useParams();
    const id = params.id as string;

    const [formData, setFormData] = useState<Partial<UpdateFormData>>({
        name: '',
        barcode: '',
        categoryId: '',
    });
    
    const [sku, setSku] = useState('');

    const [categories, setCategories] = useState<Category[]>([]);
    const [locations, setLocations] = useState<Location[]>([]);
    
    const [history, setHistory] = useState<StockTransaction[]>([]);
    
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [successMessage, setSuccessMessage ] = useState('');

    useEffect(() => {
        if(!id)
        {
            return;
        }

        const fetchData = async () => {
            setLoading(true);
            setError('');
            try {
                const [categoriesRes, productRes, historyRes, locationRes] = await Promise.all([
                    axiosInstance.get('/categories'),
                    axiosInstance.get(`/products/${id}`),
                    axiosInstance.get(`/inventory/history/${id}`),
                    locationService.getAll()
                ]);

                setCategories(categoriesRes.data);
                setHistory(historyRes.data);
                setLocations(locationRes);

                const product: Product = productRes.data;
                setFormData({
                    name: product.name,
                    barcode: product.barcode,
                    categoryId: product.categoryId
                });
                setSku(product.sku);
            } catch (err) {
                console.error(err);
                setError("Veriler tyüklenirken bir hata oluştu. Lütfen sayfayı yenileyin.");
            } finally {
                setLoading(false);
            }
        };
        fetchData();
    }, [id]);
    
    const getLocationCode = (locationId: string) => {
        const found = locations.find(l=>l.id === locationId);
        return found ? `${found.code} (${found.type})`: 'Bilinmeyen Lokasyon';
    }

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const {name, value} = e.target;
        setFormData(prev => ({...prev, [name]: value}));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        setSuccessMessage('');

        try {
            await axiosInstance.put(`/products/${id}`, {...formData, sku:sku});
            setSuccessMessage('Ürün bilgileri başarıyla güncellendi!');
            router.push('/');
            
        } catch (err:any) {
            console.error(err);
            if(err.response?.data?.errors) {
                const messages = Object.values(err.response.data.errors).flat().join(', ');
                setError(messages);
            } else {
                setError("ürün güncellenirken hata oluştu!");
            }
        } finally {
            setLoading(false);
        }
    };

    if(loading && !sku)
    {
        return (<div className="flex items-center justify-center min-h-screen">
            <div className="text-lg text-gray-600 animate-pulse">Ürün detayları yükleniyor...</div>
        </div>);
    }

    return (
        <div className="bg-gray-50 min-h-screen pb-20">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">

                <div className="flex items-center justify-between mb-8">
                    <h1 className="text-3xl font-bold text-gray-900">Ürün Yönetimi</h1>
                    <button
                        onClick={() => router.back()}
                        className="text-gray-600 hover:text-gray-900 font-medium"
                    >
                        &larr; Listeye Dön
                    </button>
                </div>

                {error && (
                    <div className="mb-6 p-4 bg-red-100 text-red-700 border-l-4 border-red-500 rounded shadow-sm">
                        <p className="font-bold">Hata</p>
                        <p>{error}</p>
                    </div>
                )}
                {successMessage && (
                    <div className="mb-6 p-4 bg-green-100 text-green-700 border-l-4 border-green-500 rounded shadow-sm">
                        <p className="font-bold">Başarılı</p>
                        <p>{successMessage}</p>
                    </div>
                )}

                <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">

                    <div className="lg:col-span-1">
                        <div className="bg-white rounded-lg shadow p-6 sticky top-6">
                            <h2 className="text-xl font-semibold text-gray-800 mb-4 border-b pb-2">Genel Bilgiler</h2>

                            <form onSubmit={handleSubmit} className="space-y-4">
                                <div>
                                    <label className="block text-sm font-medium text-gray-700">SKU (Stok Kodu)</label>
                                    <input
                                        value={sku}
                                        disabled
                                        className="mt-1 block w-full px-3 py-2 bg-gray-100 border border-gray-300 rounded-md text-gray-500 cursor-not-allowed"
                                    />
                                    <p className="text-xs text-gray-400 mt-1">SKU değiştirilemez.</p>
                                </div>

                                <div>
                                    <label className="block text-sm font-medium text-gray-700">Ürün Adı</label>
                                    <input
                                        name="name"
                                        value={formData.name}
                                        onChange={handleChange}
                                        required
                                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                                    />
                                </div>

                                <div>
                                    <label className="block text-sm font-medium text-gray-700">Barkod</label>
                                    <input
                                        name="barcode"
                                        value={formData.barcode}
                                        onChange={handleChange}
                                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                                    />
                                </div>

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

                                <div className="pt-4">
                                    <button
                                        type="submit"
                                        disabled={loading}
                                        className={`w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none ${loading ? 'opacity-50' : ''}`}
                                    >
                                        {loading ? 'Kaydediliyor...' : 'Değişiklikleri Kaydet'}
                                    </button>
                                </div>
                            </form>
                        </div>
                    </div>

                    <div className="lg:col-span-2">
                        <div className="bg-white rounded-lg shadow overflow-hidden">
                            <div className="px-6 py-4 border-b border-gray-200 flex justify-between items-center bg-gray-50">
                                <h2 className="text-xl font-semibold text-gray-800">Stok Hareket Geçmişi</h2>
                                <span className="text-sm text-gray-500">Toplam {history.length} kayıt</span>
                            </div>

                            <div className="overflow-x-auto">
                                <table className="min-w-full divide-y divide-gray-200">
                                    <thead className="bg-gray-50">
                                    <tr>
                                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Tarih</th>
                                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">İşlem / Belge</th>
                                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Lokasyon</th>
                                        <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Miktar</th>
                                    </tr>
                                    </thead>
                                    <tbody className="bg-white divide-y divide-gray-200">
                                    {history.map((tx) => (
                                        <tr key={tx.id} className="hover:bg-gray-50 transition-colors">
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                                                {new Date(tx.timestamp).toLocaleString('tr-TR', {
                                                    day: '2-digit', month: '2-digit', year: 'numeric',
                                                    hour: '2-digit', minute: '2-digit'
                                                })}
                                            </td>
                                            <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                                                {tx.sourceDocument || '-'}
                                            </td>
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">
                                                {getLocationCode(tx.locationId)}
                                            </td>
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-right font-bold">
                                                {/* Pozitif değerler Yeşil, Negatif değerler Kırmızı */}
                                                <span className={`px-2 py-1 rounded-full ${tx.quantityChange > 0 ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
                                            {tx.quantityChange > 0 ? `+${tx.quantityChange}` : tx.quantityChange}
                                        </span>
                                            </td>
                                        </tr>
                                    ))}

                                    {history.length === 0 && (
                                        <tr>
                                            <td colSpan={4} className="px-6 py-10 text-center text-gray-500 italic">
                                                Bu ürüne ait henüz bir stok hareketi bulunmuyor.
                                            </td>
                                        </tr>
                                    )}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    );
}
