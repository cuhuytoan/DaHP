import { Link, useLocation } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { cn } from '../../lib/utils';
import {
    LayoutDashboard,
    Package,
    FileText,
    ShoppingCart,
    Settings,
    LogOut,
    Menu,
    X,
} from 'lucide-react';
import { useState } from 'react';
import { Button } from '../ui/button';

const navigation = [
    { name: 'Dashboard', href: '/', icon: LayoutDashboard },
    { name: 'Products', href: '/products', icon: Package },
    { name: 'Articles', href: '/articles', icon: FileText },
    { name: 'Orders', href: '/orders', icon: ShoppingCart },
    { name: 'Settings', href: '/settings', icon: Settings },
];

export function Sidebar() {
    const location = useLocation();
    const { logout, user } = useAuth();
    const [isMobileOpen, setIsMobileOpen] = useState(false);

    return (
        <>
            {/* Mobile menu button */}
            <div className="lg:hidden fixed top-4 left-4 z-50">
                <Button
                    variant="outline"
                    size="icon"
                    onClick={() => setIsMobileOpen(!isMobileOpen)}
                >
                    {isMobileOpen ? <X className="h-5 w-5" /> : <Menu className="h-5 w-5" />}
                </Button>
            </div>

            {/* Overlay */}
            {isMobileOpen && (
                <div
                    className="lg:hidden fixed inset-0 bg-black/50 z-40"
                    onClick={() => setIsMobileOpen(false)}
                />
            )}

            {/* Sidebar */}
            <aside
                className={cn(
                    'fixed inset-y-0 left-0 z-40 w-64 bg-card border-r transform transition-transform duration-200 ease-in-out lg:translate-x-0',
                    isMobileOpen ? 'translate-x-0' : '-translate-x-full'
                )}
            >
                <div className="flex h-full flex-col">
                    {/* Logo */}
                    <div className="flex h-16 items-center px-6 border-b">
                        <h1 className="text-xl font-bold text-primary">CMS Admin</h1>
                    </div>

                    {/* Navigation */}
                    <nav className="flex-1 space-y-1 px-4 py-4">
                        {navigation.map((item) => {
                            const isActive = location.pathname === item.href;
                            return (
                                <Link
                                    key={item.name}
                                    to={item.href}
                                    onClick={() => setIsMobileOpen(false)}
                                    className={cn(
                                        'flex items-center gap-3 rounded-lg px-3 py-2 text-sm font-medium transition-colors',
                                        isActive
                                            ? 'bg-primary text-primary-foreground'
                                            : 'text-muted-foreground hover:bg-accent hover:text-accent-foreground'
                                    )}
                                >
                                    <item.icon className="h-5 w-5" />
                                    {item.name}
                                </Link>
                            );
                        })}
                    </nav>

                    {/* User section */}
                    <div className="border-t p-4">
                        <div className="flex items-center gap-3 mb-3">
                            <div className="h-10 w-10 rounded-full bg-primary flex items-center justify-center">
                                <span className="text-primary-foreground font-medium">
                                    {user?.fullName?.charAt(0) || 'U'}
                                </span>
                            </div>
                            <div className="flex-1 min-w-0">
                                <p className="text-sm font-medium truncate">{user?.fullName}</p>
                                <p className="text-xs text-muted-foreground truncate">{user?.email}</p>
                            </div>
                        </div>
                        <Button
                            variant="outline"
                            className="w-full justify-start gap-2"
                            onClick={logout}
                        >
                            <LogOut className="h-4 w-4" />
                            Đăng xuất
                        </Button>
                    </div>
                </div>
            </aside>
        </>
    );
}
