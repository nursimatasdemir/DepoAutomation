'use client'

import React from 'react'

export const OperatorDashboard = () => {
    
    const tasks = [
        {id:1, type: "Mal Kabul", document: "PO_1004", status: "Bekliyor"},
        { id: 2, type: "Transfer", document: "Yerlestirme-003", status: "Bekliyor" },
        { id: 3, type: "Stok Toplama", document: "SO-5002", status: "Bekliyor" },    
    ];

    return (
        <div>
            <h2 className="text-3xl font-bold text-gray-900 mb-6">
                Operatör İş Listesi
            </h2>

            <div className="bg-white shadow overflow-hidden sm:rounded-lg">
                <table className="min-w-full divide-y divide-gray-200">
                    <thead className="bg-gray-50">
                    <tr>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">İş Tipi</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Belge No</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Durum</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">İşlem</th>
                    </tr>
                    </thead>
                    <tbody className="bg-white divide-y divide-gray-200">
                    {tasks.map((task) => (
                        <tr key={task.id}>
                            <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{task.type}</td>
                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{task.document}</td>
                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                  <span className="px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-yellow-100 text-yellow-800">
                    {task.status}
                  </span>
                            </td>
                            <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                                <a href="#" className="text-indigo-600 hover:text-indigo-900">
                                    Başla
                                </a>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
            
        </div>
    );
}