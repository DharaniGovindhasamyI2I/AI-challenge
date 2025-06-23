import createApiClient from './apiFactory';

const api = createApiClient('http://localhost:5031/api/');

export const getProducts = () => api.get('product');

export default api; 