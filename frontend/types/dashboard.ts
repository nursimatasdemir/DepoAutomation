export interface CatalogStats {
    productCount: number,
    categoryCount: number,
    locationCount: number,
    activeProductCount: number
}

export interface InventoryStats {
    totalTransactions: number,
    incomingTransactions: number,
    outgoingTransactions: number,
    totalItemsInStock: number,
}