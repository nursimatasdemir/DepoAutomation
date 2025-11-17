'use client'

import {useEffect, useState} from 'react';
import {useRouter} from 'next/navigation';

import productService from '@/services/productService';
import locationService from '@/services/locationService';
import inventoryService from '@/services/inventoryService';
import {Location} from '@/types/location';

import {Product} from '@/types/product';

export default function TransferStockPage() {
    const router = useRouter();
    
    const [formData, setFormData] = useState({
        productId: '',
        sourceLocationId: '',
        destinationLocationId: '',
        quantityToTransfer: 0,
        sourceDocument: ''
    });

    const [products, setProducts] = useState<Product[]>([]);
    const [locations, setLocations] = useState<Location[]>([]);

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    
    useEffect(() => {
        const fetchDataForDropdowns = async () => {
            try {
                const [productsRes, locationsRes] = await Promise.all([
                    productService.getAll(),
                    locationService.getAll()
                ]);
                setProducts(productsRes);
                setLocations(locationsRes);
                
            } catch (err) {
                console.error(err);
                setError("Form verileri (Ürün/Lokasyon) yüklenemedi!");
            }
        };
        fetchDataForDropdowns();
    }, []);
    
    const  handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const {name, value} = e.target;
        
        setFormData(prev => ({...prev, [name]: name === 'quantityToTransfer' ? parseFloat(value) : value}));
    };
    
    const handleSubmit = async (e:React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        
        try {
            if (formData.quantityToTransfer <= 0)
            {
                setError("Miktar 0'dan büyük olmalıdır.");
                setLoading(false);
                return;
            }
            
            await inventoryService.transferStock(formData);
            alert('Transfer işlemi başarıyla kaydedildi!');
            router.push('/');
        } catch (err:any) {
            console.error(err);
            if (err.response?.status === 400) {
                setError(err.response.data.detail || err.response.data.title || "İşlem başarısız. Kaynak stok yetersiz olabilir.");
            } else {
                setError("Transfer işlemi sırasında bir hata oluştu.");
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen bg-gray-100 py-10 px-4 sm:px-6 lg:px-8">
            <div className="max-w-md mx-auto bg-white rounded shadow p-6">
                <h2 className="text-2xl font-bold mb-6 text-gray-900">Depo İçi Transfer</h2>

                {error && (
                    <div className="mb-4 p-3 bg-red-100 text-red-700 border border-red-400 rounded text-sm">
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit} className="space-y-4">

                    {/* Ürün Seçimi (Dropdown) */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Hangi Ürün?</label>
                        <select
                            name="productId"
                            value={formData.productId}
                            onChange={handleChange}
                            required
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                        >
                            <option value="">Ürün Seçiniz...</option>
                            {products.map(p => (
                                <option key={p.id} value={p.id}>{p.name} ({p.sku})</option>
                            ))}
                        </select>
                    </div>

                    {/* Kaynak Lokasyon Seçimi (Dropdown) */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">NEREDEN? (Kaynak Lokasyon)</label>
                        <select
                            name="sourceLocationId"
                            value={formData.sourceLocationId}
                            onChange={handleChange}
                            required
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                        >
                            <option value="">Kaynak Seçiniz...</option>
                            {locations.map(l => (
                                <option key={l.id} value={l.id}>{l.code} ({l.type})</option>
                            ))}
                        </select>
                    </div>

                    {/* Hedef Lokasyon Seçimi (Dropdown) */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">NEREYE? (Hedef Lokasyon)</label>
                        <select
                            name="destinationLocationId"
                            value={formData.destinationLocationId}
                            onChange={handleChange}
                            required
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                        >
                            <option value="">Hedef Seçiniz...</option>
                            {locations.map(l => (
                                <option key={l.id} value={l.id}>{l.code} ({l.type})</option>
                            ))}
                        </select>
                    </div>

                    {/* Miktar */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Taşınacak Miktar</label>
                        <input
                            name="quantityToTransfer"
                            type="number"
                            value={formData.quantityToTransfer}
                            onChange={handleChange}
                            required
                            min="0.01"
                            step="any"
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                            placeholder="0"
                        />
                    </div>

                    {/* Kaynak Belge */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Kaynak Belge (Transfer Emri No)</label>
                        <input
                            name="sourceDocument"
                            value={formData.sourceDocument}
                            onChange={handleChange}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 text-gray-900"
                            placeholder="Yerlestirme-001"
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
                            className={`px-4 py-2 text-sm font-medium text-white bg-indigo-600 rounded-md hover:bg-indigo-700 ${loading ? 'opacity-50' : ''}`}
                        >
                            {loading ? 'Taşınıyor...' : 'Transferi Kaydet'}
                        </button>
                    </div>

                </form>
            </div>
        </div>
    );
}