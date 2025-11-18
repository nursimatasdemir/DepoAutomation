'use client'

import React from 'react';
import { useRouter } from 'next/navigation';
import { useState, useEffect } from 'react';

const IconDashboard = () => <span>📊</span>;
const IconProduct = () => <span>📦</span>;
const IconReceive = () => <span>🏭</span>;
const IconTransfer = () => <span>🚚</span>;
const IconStockOut = () => <span>🛒</span>;
const IconCategory = () => <span>🗂️</span>;
    
const Sidebar = () => {
    return (
        <div className="w-64 bg-white shadow-md h-screen fixed top-0 left-0 pt-16">
            <div className="px-6 py-4">
                <h2 className="text-xs font-semibold text-gray-500 uppercase tracking-wider">DEPO</h2>
            </div>
            <nav className="mt-2">
                {/* Navigasyon Linkleri */}
                <a href="/"
                   className="flex items-center px-6 py-3 text-gray-700 hover:bg-gray-100">
                    <IconDashboard/>
                    <span className="ml-3">Ana Panel</span>
                </a>
                <a href="/products/new"
                   className="flex items-center px-6 py-3 text-gray-700 hover:bg-gray-100">
                    <IconProduct/>
                    <span className="ml-3">Ürün Ekle</span>
                </a>
                <a href="/inventory/receive"
                   className="flex items-center px-6 py-3 text-gray-700 hover:bg-gray-100">
                    <IconReceive/>
                    <span className="ml-3">Mal Kabul</span>
                </a>
                <a href="/inventory/transfer"
                   className="flex items-center px-6 py-3 text-gray-700 hover:bg-gray-100">
                    <IconTransfer/>
                    <span className="ml-3">Transfer</span>
                </a>
                <a href="/inventory/pick"
                   className="flex items-center px-6 py-3 text-gray-700 hover:bg-gray-100">
                    <IconStockOut/>
                    <span className="ml-3">Stok Çıkışı</span>
                </a>
                <a href="/categories"
                   className="flex items-center px-6 py-3 text-gray-700 hover:bg-gray-100">
                    <IconCategory/>
                    <span className="ml-3">Kategoriler</span>
                </a>
            </nav>
        </div>
    );
};

const Topbar = () => {
    const router = useRouter();
    const [userName, setUserName] = useState<string | null>(null);

    useEffect(() => {
        const userName = localStorage.getItem("userName");
        if (userName) {
            setUserName(userName);
        }
    }, []);
    
    const handleLogout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('userRole');
        localStorage.removeItem('userName');
        router.push('/login');
    };

    return (
        <div className="h-16 bg-white shadow-sm fixed top-0 left-64 right-0 z-10">
            <div className="flex items-center justify-end h-full px-6">
                <span className="mr-4 text-sm text-gray-600">Hoş geldin, {userName ?? 'Kullanıcı'}</span>
                <button
                    onClick={handleLogout}
                    className="text-gray-600 hover:text-red-600 font-medium text-sm"
                >
                    Çıkış Yap
                </button>
            </div>
        </div>
    );
};

export default function AdminLayout({
    children,
}: {
    children:React.ReactNode;
}) {
    const router = useRouter();
    
    useEffect(() => {
        const token = localStorage.getItem('token');
        if(!token){
            router.push('/login');
        }
    }, [router]);

    return (
        <div className="flex min-h-screen bg-gray-100">
            <Sidebar />
            <div className="flex-1 flex flex-col ml-64"> 
                <Topbar />

                <main className="flex-1 p-6 pt-22"> 
                    {children} 
                </main>
            </div>
        </div>
    );
}
    