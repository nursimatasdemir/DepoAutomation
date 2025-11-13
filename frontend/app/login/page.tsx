'use client'

import { useState } from 'react';
import axios from 'axios';
import {useRouter} from 'next/navigation';

export default function LoginPage() {
    const router = useRouter();
    
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    
    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        
        try {
            const response = await axios.post('http://localhost:5195/api/Auth/login', {
                username: username,
                password: password,
            });
            
            const token = response.data.token;
            localStorage.setItem('token', token);
            
            alert('Giriş başarılı!');
            router.push('/');
        } catch (err: any) {
            console.error(err);
            
            if (err.response && err.response.status === 400) {
                setError('Kullanıcı adı veya şifre hatalı!');
            } else {
                setError('Sunucuya bağlanılamadı veya bir hata oluştu.');
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="flex min-h-screen items-center justify-center bg-gray-100">
            <div className="w-full max-w-md p-8 space-y-6 bg-white rounded shadow-md">
                <h2 className="text-2xl font-bold text-center text-gray-900">
                    Depo Giriş
                </h2>

                {error && (
                    <div className="p-3 text-sm text-red-700 bg-red-100 rounded border border-red-400">
                        {error}
                    </div>
                )}

                <form onSubmit={handleLogin} className="space-y-6">
                    <div>
                        <label htmlFor="username" className="block text-sm font-medium text-gray-900">
                            Kullanıcı Adı
                        </label>
                        <input
                            id="username"
                            type="username"
                            required
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            className="w-full px-3 py-2 mt-1 border border-gray-300 rounded focus:outline-none focus:ring-indigo-400 focus:border-indigo-500 placeholder:text-gray-500 text-gray-900"
                            placeholder="username"
                        />
                    </div>

                    <div>
                        <label htmlFor="password" className="block text-sm font-medium text-gray-900">
                            Şifre
                        </label>
                        <input
                            id="password"
                            type="password"
                            required
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            className="w-full px-3 py-2 mt-1 border border-gray-300 rounded focus:outline-none focus:ring-indigo-400 focus:border-indigo-500 placeholder:text-gray-500 text-gray-900"
                            placeholder="******"
                        />
                    </div>

                    <button
                        type="submit"
                        disabled={loading}
                        className={`w-full py-2 px-4 text-white bg-blue-500 rounded hover:bg-blue-700 focus:outline-none ${
                            loading ? 'opacity-50 cursor-not-allowed' : ''
                        }`}
                    >
                        {loading ? 'Giriş Yapılıyor...' : 'Giriş Yap'}
                    </button>
                </form>
            </div>
        </div>
    );
}