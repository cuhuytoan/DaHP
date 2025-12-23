import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { AuthProvider } from './context/AuthContext';
import { Layout } from './components/layout/Layout';
import { LoginPage } from './pages/auth/LoginPage';
import { DashboardPage } from './pages/dashboard/DashboardPage';
import { ProductsPage } from './pages/products/ProductsPage';
import { ProductFormPage } from './pages/products/ProductFormPage';
import { ArticlesPage } from './pages/articles/ArticlesPage';
import { ArticleFormPage } from './pages/articles/ArticleFormPage';
import { OrdersPage } from './pages/orders/OrdersPage';
import { ArticleCategoriesPage } from './pages/categories/ArticleCategoriesPage';
import { ProductCategoriesPage } from './pages/categories/ProductCategoriesPage';
import { ProductBrandsPage } from './pages/brands/ProductBrandsPage';
import { AdvertisingPage } from './pages/advertising/AdvertisingPage';
import { UsersPage } from './pages/users/UsersPage';
import { SettingsPage } from './pages/settings/SettingsPage';
import { CommentsPage } from './pages/comments/CommentsPage';
import './index.css';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,
      refetchOnWindowFocus: false,
    },
  },
});

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route element={<Layout />}>
              <Route path="/" element={<DashboardPage />} />
              {/* Products */}
              <Route path="/products" element={<ProductsPage />} />
              <Route path="/products/new" element={<ProductFormPage />} />
              <Route path="/products/:id/edit" element={<ProductFormPage />} />
              <Route path="/products/categories" element={<ProductCategoriesPage />} />
              {/* Articles */}
              <Route path="/articles" element={<ArticlesPage />} />
              <Route path="/articles/new" element={<ArticleFormPage />} />
              <Route path="/articles/:id/edit" element={<ArticleFormPage />} />
              <Route path="/articles/categories" element={<ArticleCategoriesPage />} />
              {/* Orders */}
              <Route path="/orders" element={<OrdersPage />} />
              {/* Brands */}
              <Route path="/brands" element={<ProductBrandsPage />} />
              {/* Advertising */}
              <Route path="/advertising" element={<AdvertisingPage />} />
              {/* Comments */}
              <Route path="/comments" element={<CommentsPage />} />
              {/* Users */}
              <Route path="/users" element={<UsersPage />} />
              {/* Settings */}
              <Route path="/settings" element={<SettingsPage />} />
            </Route>
          </Routes>
        </BrowserRouter>
      </AuthProvider>
    </QueryClientProvider>
  );
}

export default App;
