'use client';

import { useEffect, useState } from 'react';
import {useRouter} from 'next/navigation';

import productService from '@/services/productService';
import inventoryService from '@/services/inventoryService';

import {StockLevelDTO} from '@/types/inventory';
import { Product } from '@/types/product';

import axiosInstance from '@/utils/axiosInstance';

import {AdminDashboard} from '@/components/dashboards/AdminDashboard';
import {OperatorDashboard} from '@/components/dashboards/OperatorDashboard';

interface ProductWithStock extends Product {
  quantity: number;
}

export default function Dashboard() {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  
  const [userRole, setUserRole] = useState<string | null>(null);

  useEffect(() => {
    const role = localStorage.getItem('userRole');
    setUserRole(role);
    setLoading(false);
  }, []);
  
  
  if (loading) return <div className="p-10 text-center">Yükleniyor...</div>;
  
  return (
      <div>
        {/* Eğer rol 'Admin' ise, AdminDashboard'u göster */}
        {userRole === 'Admin' && (
            <AdminDashboard />
        )}

        {/* Eğer rol 'Operator' ise, OperatorDashboard'u göster */}
        {userRole === 'Operator' && (
            <OperatorDashboard />
        )}

        {/* Eğer rol ikisi de değilse (beklenmedik durum), bir hata göster */}
        {userRole !== 'Admin' && userRole !== 'Operator' && (
            <div className="bg-red-100 border-l-4 border-red-500 text-red-700 p-4">
              <p>Hata: Geçerli bir kullanıcı rolü bulunamadı.</p>
            </div>
        )}
      </div>
  );
}