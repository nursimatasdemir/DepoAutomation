'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import productService from '../services/productService';
import { Product } from '../types/product';
import axiosInstance from '../utils/axiosInstance';

export default function Dashboard() {
  const router = useRouter();
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) {
      router.push('/login');
      return;
    }
    fetchProducts();
  }, [router]);

  const fetchProducts = async () => {
    try {
      const data = await productService.getAll();
      setProducts(data);
    } catch (err) {
      console.error('Ürünler çekilemedi:', err);
      setError('Ürün listesi yüklenirken bir hata oluştu.');
    } finally {
      setLoading(false);
    }
  };
  
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
  
  const handleLogout = () => {
    localStorage.removeItem('token');
    router.push('/login');
  };

  if (loading) return <div className="p-10 text-center">Yükleniyor...</div>;

  return (
      <div className="min-h-screen bg-gray-100">
        {/* Üst Menü */}
        <nav className="bg-white shadow-sm">
          <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div className="flex justify-between h-16">
              <div className="flex items-center">
                <h1 className="text-xl font-bold text-blue-600">Depo</h1>
              </div>
              <div className="flex items-center">
                <button
                    onClick={handleLogout}
                    className="text-gray-600 hover:text-red-600 font-medium"
                >
                  Çıkış Yap
                </button>
              </div>
            </div>
          </div>
        </nav>

        {/* Ana İçerik */}
        <main className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
          <div className="px-4 py-6 sm:px-0">
            <div className="flex justify-between items-center mb-6">
              <h2 className="text-2xl font-bold text-gray-800">Ürün Listesi</h2>
              <button 
                  onClick={() => router.push('/products/new')}
                  className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-700">
                + Yeni Ürün
              </button>
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
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">SKU</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Ürün Adı</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Barkod</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Kategori</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Durum</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">İşlemler</th>
                </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                {products.map((product) => (
                    <tr key={product.id}>
                      <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{product.sku}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{product.name}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{product.barcode}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{product.categoryName}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      <span
                          className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${product.isActive ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
                        {product.isActive ? 'Aktif' : 'Pasif'}
                      </span>
                      </td>
                      <td className="px-6 py-4 inline-flex leading-5 text-sm font-semibold rounded-full bg-red-100 text-green-800">
                        <button
                            onClick={() => handleDelete(product.id)}
                            className="text-red-600 hover:text-red-900"
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
        </main>
      </div>
  );
}