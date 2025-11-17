'use client'

import {useEffect, useState} from 'react';
import {useRouter} from 'next/navigation';

import productService from '@/services/productService';
import locationService from '@/services/locationService';
import inventoryService from '@/services/inventoryService';
import {Location} from '@/types/location';

import {Product} from '@/types/product';

export default function ReceiveStockPage() {
    const router = useRouter();
    
    const [formData, setFormData] = useState({
        productId: '',
        locationId: '',
        quantityReceived: 0,
        sourceDocument: ''
    });
    
    const [products, setProducts] = useState<Product[]>([]);
    const [locations, setLocations] = useState<Location[]>([]);
    
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    
    const [receivingLocationId, setReceivingLocationId] = useState<string | null>(null);
    
    useEffect(() => {
        const fetchDataForDropdowns = async () => {
            try {
                const [productsRes, locationsRes] = await Promise.all([
                    productService.getAll(),
                    locationService.getAll()
                ]);
                setProducts(productsRes);
                setLocations(locationsRes);
                
                const kabulAlani = locationsRes.find(l=>l.type.toLowerCase() === 'kabulalani' || l.code.toLowerCase() === 'rec-01');
                if(kabulAlani)
                {
                    setReceivingLocationId(kabulAlani.id);
                    setFormData(prev=>({...prev, locationId:kabulAlani.id}));
                } else {
                    setError("Kritik Hata: Sistemde 'KabulAlani' tipinde bir lokasyon tanımlanmamış. Lütfen bir admin ile görüşün.");
                }
            } catch (err) {
                console.error(err);
                setError("Form verileri (Ürün/Lokasyon) yüklenemedi!");
            }
        };
        fetchDataForDropdowns();
    }, []);
    
    const handleChange = (e:React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const {name, value} = e.target;
        setFormData(prev=>({...prev, [name]: name === 'quantityReceived' ? parseFloat(value): value}));
    }
    
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!formData.locationId || formData.locationId !== receivingLocationId) {
            setError("Geçerli bir kabul alanı lokasyonu seçilmedi. Sayfayı yenileyin.");
            return;
        }
        
        setLoading(true);
        setError('');
        
        try {
            if(formData.quantityReceived <= 0) {
                setError("Miktar 0'dan büyük olmalıdır.");
                setLoading(false);
                return;
            }
            
            await inventoryService.receiveStock(formData);
            alert('Mal kabul işlemi başarıyla gerçekleştirildi.')
            router.push('/');
            
        } catch (err:any) {
            console.error(err);
            if (err.response?.data?.errors) {
                const messages = Object.values(err.response?.data?.errors).flat().join(', ');
                setError(messages);
            } else {
                setError("Mal kabul işlemi sırasında bir hata oluştu");
            }
        } finally {
            setLoading(false);
        }
    };
    
    return (
        <div className="min-h-screen bg-gray-100 py-10 px-4 sm:px-6 lg:px-8">
            <div className="max-w-md mx-auto bg-white rounded shadow p-6">
                <h2 className="text-2xl font-bold mb-6 text-gray-900">Mal Kabul (Giriş)</h2>

                {error && (
                    <div className="mb-4 p-3 bg-red-100 text-red-700 border border-red-400 rounded text-sm">
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit} className="space-y-4">

                    {/* Ürün Seçimi (Dropdown) */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Ürün</label>
                        <select
                            name="productId" // <-- Bu 'name', 'formData' state'indeki 'productId' ile eşleşmeli
                            value={formData.productId}
                            onChange={handleChange}
                            required
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                        >
                            <option value="">Ürün Seçiniz...</option>
                            {/* 'products' state'ini 'map' (dön) ile <option> elemanlarına çevir */}
                            {products.map(p => (
                                <option key={p.id} value={p.id}>{p.name} ({p.sku})</option>
                            ))}
                        </select>
                    </div>

                    {/* Lokasyon Seçimi (Dropdown) */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Hedef Lokasyon (Giriş Yeri)</label>
                        <select
                            name="locationId"
                            value={formData.locationId} // <-- State'den otomatik seçili gelir
                            onChange={handleChange}
                            required
                            disabled={true} // <-- KİLİTLENDİ (Değiştirilemez)
                            // <-- Görünümü soluklaştır ve tıklanamaz yap
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-500 bg-gray-100 cursor-not-allowed"
                        >
                            {/* Listede tüm lokasyonları gösteririz, ancak 'value'
                state'den ('kabulAlani.id') geldiği ve 'disabled' olduğu için
                sadece "KabulAlani" seçili kalır.
              */}
                            <option value="">Yükleniyor...</option>
                            {locations.map(l => (
                                <option key={l.id} value={l.id}>{l.code} ({l.type})</option>
                            ))}
                        </select>
                    </div>

                    {/* Miktar */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Gelen Miktar</label>
                        <input
                            name="quantityReceived" // <-- 'formData' state'i ile eşleşmeli
                            type="number"
                            value={formData.quantityReceived}
                            onChange={handleChange}
                            required
                            min="0.01" // 0'dan büyük olmalı
                            step="any" // Ondalıklı sayılara izin ver (kg vb. için)
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                            placeholder="0"
                        />
                    </div>

                    {/* Kaynak Belge */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Kaynak Belge (İrsaliye/PO No)</label>
                        <input
                            name="sourceDocument" // <-- 'formData' state'i ile eşleşmeli
                            value={formData.sourceDocument}
                            onChange={handleChange}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                            placeholder="PO-1001"
                        />
                    </div>

                    <div className="flex justify-end space-x-3 pt-4">
                        <button
                            type="button"
                            onClick={() => router.back()} // Bir önceki sayfaya (Ana Sayfa) git
                            className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50"
                        >
                            İptal
                        </button>
                        <button
                            type="submit"
                            disabled={loading} // Yüklenirken butonu devre dışı bırak
                            className={`px-4 py-2 text-sm font-medium text-white bg-indigo-600 rounded-md hover:bg-indigo-700 ${loading ? 'opacity-50' : ''}`}
                        >
                            {loading ? 'Kaydediliyor...' : 'Stok Girişi Yap'}
                        </button>
                    </div>

                </form>
            </div>
        </div>
    );
    
}