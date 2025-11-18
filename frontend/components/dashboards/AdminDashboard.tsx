'use client'

import React from 'react';

export const AdminDashboard = () => {
    return (
        <div>
            <h2 className="text-3xl font-bold text-gray-900 mb-6">
                Admin Paneli
            </h2>
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
            <div className="mt-8 bg-white p-6 rounded-lg shadow">
                <h3 className="text-lg font-medium text-gray-900">Aylık Satış Grafiği</h3>
                <p className="mt-4 text-gray-500">[Grafik buraya gelecek]</p>
            </div>
        </div>
    );
};
