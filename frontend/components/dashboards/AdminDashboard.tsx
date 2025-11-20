'use client'

import React, {useEffect, useState } from 'react';

import productService from '@/services/productService';
import { StockLevelDTO } from '@/types/inventory';
import inventoryService from '@/services/inventoryService'; 
import { Product } from '@/types/product';
import { useRouter } from 'next/navigation';

import {
    BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer
} from 'recharts';

import dashboardService from '@/services/dashboardService';
import {CatalogStats, InventoryStats} from '@/types/dashboard';

interface ProductWithStock extends Product {
    quantity: number; 
}

export const AdminDashboard = () => {
    const router = useRouter();

    const [products, setProducts] = useState<ProductWithStock[]>([]);
    const [searchTerm, setSearchTerm] = useState('');
    const [showArchived, setShowArchived] = useState(false);

    const [catalogStats, setCatalogStats] = useState<CatalogStats | null>(null);
    const [inventoryStats, setInventoryStats] = useState<InventoryStats | null>(null);
    
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        fetchData();
        fetchStats();
    }, [showArchived]);

    const fetchData = async () => {
        setLoading(true);

        try {
            const [productsRes, stockLevelsRes] = await Promise.all([
                productService.getAll(searchTerm, showArchived),
                inventoryService.getAllStockLevels()
            ]);

            const stockMap = new Map<string, number>(
                stockLevelsRes.map((stock => [stock.productId, stock.totalQuantity]))
            );

            const combinedData: ProductWithStock[] = productsRes.map(product=>({
                ...product,
                quantity: stockMap.get(product.id) ?? 0
            }));
            setProducts(combinedData);
            
        } catch (err:any) {
            console.error('Veri çekilemedi', err);
            setError("Ürün veya stok listesi yüklenirken bir hata oluştu.");
        } finally {
            setLoading(false);
        }
    }

    const fetchStats = async () => {
        try {
            const [catStats, invStats] = await Promise.all([
                dashboardService.getCatalogStats(),
                dashboardService.getInventoryStats()
            ]);
            setCatalogStats(catStats);
            setInventoryStats(invStats);
        } catch (err) {
            console.error("İstatistikler yüklenemedi", err);
        }
    };

    const chartData = [
        { name: 'Giriş', miktar: inventoryStats?.incomingTransactions || 0 },
        { name: 'Çıkış', miktar: inventoryStats?.outgoingTransactions || 0 },
        { name: 'Toplam İşlem', miktar: inventoryStats?.totalTransactions || 0 },
    ];

    const handleDelete = async (productId: string) => {
        if(!window.confirm("Bu ürünü arşivlemek istediğinizden emin misiniz?"))
        {
            return;
        }

        try {
            await productService.delete(productId);
            setProducts(prevProducts => prevProducts.filter(product => product.id !== productId));
            alert("Ürün başarıyla kaldırıldı!");
        } catch (err:any) {
            console.error("Ürün silinemedi: ",err);

            if(err.response && err.response.status === 403)
            {
                setError("Bu işlem için 'Admin' yetkisine sahip olmalısınız!");
            }
        }
    };
    
    return (
        <div>
            <h2 className="text-3xl font-bold text-gray-900 mb-6">
                Admin Paneli
            </h2>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">

                <div className="bg-white p-6 rounded-lg shadow border-l-4 border-indigo-500">
                    <h3 className="text-sm font-medium text-gray-500">Toplam Ürün</h3>
                    <p className="mt-2 text-3xl font-bold text-gray-900">
                        {catalogStats?.productCount ?? '-'}
                    </p>
                    <span className="text-xs text-green-600">
            {catalogStats?.activeProductCount} Aktif
          </span>
                </div>

                <div className="bg-white p-6 rounded-lg shadow border-l-4 border-green-500">
                    <h3 className="text-sm font-medium text-gray-500">Toplam Stok Adedi</h3>
                    <p className="mt-2 text-3xl font-bold text-gray-900">
                        {inventoryStats?.totalItemsInStock ?? '-'}
                    </p>
                </div>

                <div className="bg-white p-6 rounded-lg shadow border-l-4 border-yellow-500">
                    <h3 className="text-sm font-medium text-gray-500">Kategoriler</h3>
                    <p className="mt-2 text-3xl font-bold text-gray-900">
                        {catalogStats?.categoryCount ?? '-'}
                    </p>
                </div>

                <div className="bg-white p-6 rounded-lg shadow border-l-4 border-red-500">
                    <h3 className="text-sm font-medium text-gray-500">Lokasyonlar</h3>
                    <p className="mt-2 text-3xl font-bold text-gray-900">
                        {catalogStats?.locationCount ?? '-'}
                    </p>
                </div>

            </div>

            <div className="bg-white p-6 rounded-lg shadow mb-8">
                <h3 className="text-lg font-bold text-gray-800 mb-4">Depo Hareket Özeti</h3>
                <div className="h-64 w-full">
                    <ResponsiveContainer width="100%" height="100%">
                        <BarChart data={chartData}>
                            <CartesianGrid strokeDasharray="3 3"/>
                            <XAxis dataKey="name"/>
                            <YAxis/>
                            <Tooltip/>
                            <Legend/>
                            <Bar dataKey="miktar" fill="#4F46E5" name="İşlem Sayısı"/>
                        </BarChart>
                    </ResponsiveContainer>
                </div>
            </div>
            
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                <div className="bg-white p-6 rounded-lg shadow">
                    <h3 className="text-sm font-medium text-gray-500">Toplam Ürün Çeşidi</h3>
                    <p className="mt-2 text-3xl font-semibold text-gray-900">124</p>
                </div>
                <div className="bg-white p-6 rounded-lg shadow">
                    <h3 className="text-sm font-medium text-gray-500">Kritik Stok Seviyesi</h3>
                    <p className="mt-2 text-3xl font-semibold text-red-600">5 Ürün</p>
                </div>
            </div>
            <div className="px-4 py-6 sm:px-0">
                <div className="flex justify-between items-center mb-6">
                    <h2 className="text-2xl font-bold text-gray-800">Envanter Durumu (Tüm Ürünler)</h2>
                    <button
                        onClick={() => router.push('/products/new')}
                        className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-indigo-700"
                    >
                        + Yeni Ürün
                    </button>
                </div>
                <div className="bg-white p-4 rounded-lg shadow mb-6 flex flex-col sm:flex-row gap-4 items-center">

                    <div className="flex-1 w-full">
                        <input
                            type="text"
                            placeholder="Ürün adı veya SKU ara..."
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)}
                            onKeyDown={(e) => e.key === 'Enter' && fetchData()}
                            className="w-full px-4 py-2 border border-gray-300 rounded-md text-gray-800 focus:ring-indigo-500 focus:border-indigo-500"
                        />
                    </div>

                    {/* Ara Butonu */}
                    <button
                        onClick={fetchData}
                        className="px-4 py-2 bg-gray-100 text-gray-700 rounded-md hover:bg-gray-200 font-medium"
                    >
                        Ara 🔍
                    </button>

                    <label className="flex items-center space-x-2 cursor-pointer select-none">
                        <input
                            type="checkbox"
                            checked={showArchived}
                            onChange={(e) => setShowArchived(e.target.checked)}
                            className="h-5 w-5 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded"
                        />
                        <span className="text-gray-700 font-medium">Arşivlenenleri Göster</span>
                    </label>

                </div>

                {error && (
                    <div className="bg-red-100 border-l-4 border-red-500 text-red-700 p-4 mb-4">
                        {error}
                    </div>
                )}

                {/* Tablo */}
                <div className="bg-white shadow overflow-hidden sm:rounded-lg">
                    <table className="min-w-full divide-y divide-gray-200">
                        <thead className="bg-gray-50">
                        <tr>
                            <th className="px-6 py-3 text-left text-base font-medium text-gray-500 uppercase tracking-wider">SKU</th>
                            <th className="px-6 py-3 text-left text-base font-medium text-gray-500 uppercase tracking-wider">Ürün
                                Adı
                            </th>
                            <th className="px-6 py-3 text-left text-base font-medium text-gray-500 uppercase tracking-wider">Kategori</th>
                            <th className="px-6 py-3 text-left text-base font-medium text-gray-500 uppercase tracking-wider">Mevcut
                                Stok
                            </th>
                            <th className="px-6 py-3 text-left text-base font-medium text-gray-500 uppercase tracking-wider">Durum</th>
                            <th className="px-6 py-3 text-left text-base font-medium text-gray-500 uppercase tracking-wider">İşlemler</th>
                        </tr>
                        </thead>
                        <tbody className="bg-white divide-y divide-gray-200">
                        {products.map((product) => (
                            <tr key={product.id}>
                                <td className="px-6 py-4 whitespace-nowrap text-base font-bold text-neutral-950">{product.sku}</td>
                                <td className="px-6 py-4 whitespace-nowrap text-base  text-stone-800">{product.name}</td>
                                <td className="px-6 py-4 whitespace-nowrap text-base  text-stone-800">{product.categoryName}</td>
                                <td className="px-6 py-4 whitespace-nowrap text-base  text-stone-800">{product.quantity}</td>
                                <td className="px-6 py-4 whitespace-nowrap text-base  text-stone-800">
                  <span
                      className={`px-6 py-4 inline-flex leading-5 whitespace-nowrap text-xl font-semibold rounded-full ${product.isActive ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
                    {product.isActive ? 'Aktif' : 'Pasif'}
                  </span>
                                </td>
                                <td className="px-6 py-4 inline-flex whitespace-nowrap text-lg font-semibold text-center">
                                    <button
                                        onClick={() => router.push(`/products/${product.id}`)}
                                        className="px-6 py-4 inline-flex text-lg leading-5 font-semibold rounded-full bg-indigo-100 text-indigo-800 hover:bg-indigo-200 mr-4 "
                                    >
                                        🔎
                                    </button>
                                </td>
                                <td className="px-6 py-4 inline-flex whitespace-nowrap text-lg font-semibold text-center ">
                                    <button
                                        onClick={() => handleDelete(product.id)}
                                        className="px-6 py-4 inline-flex text-lg leading-5 font-semibold rounded-full bg-red-100 text-red-700 hover:bg-red-100 mr-4"
                                    >
                                        🗑️
                                    </button>
                                </td>
                            </tr>
                        ))}
                        </tbody>
                    </table>
                    {products.length === 0 && !error && (
                        <div className="text-center py-10 text-gray-500">Hiç ürün bulunamadı.</div>
                    )}
                </div>
            </div>
        </div>

    );
};
