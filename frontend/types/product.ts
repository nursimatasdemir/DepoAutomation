export interface Product {
    id: string;
    sku: string;
    name: string;
    barcode: string;
    categoryId: string;
    categoryName?: string;
    isActive: boolean;
}

export interface CreateProductRequest {
    sku: string;
    name: string;
    barcode: string;
    categoryId: string;
}