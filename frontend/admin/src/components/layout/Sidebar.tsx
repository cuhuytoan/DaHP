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
    ChevronDown,
    Tag,
    Users,
    MessageSquare,
    Megaphone,
} from 'lucide-react';
import { useState } from 'react';
import { Button } from '../ui/button';

interface NavItem {
    name: string;
    href?: string;
    icon: React.ElementType;
    children?: { name: string; href: string }[];
}

const navigation: NavItem[] = [
    { name: 'Dashboard', href: '/', icon: LayoutDashboard },
    {
        name: 'Bài viết',
        icon: FileText,
        children: [
            { name: 'Tất cả bài viết', href: '/articles' },
            { name: 'Danh mục', href: '/articles/categories' },
            { name: 'Blocks', href: '/articles/blocks' },
        ],
    },
    {
        name: 'Sản phẩm',
        icon: Package,
        children: [
            { name: 'Tất cả sản phẩm', href: '/products' },
            { name: 'Danh mục', href: '/products/categories' },
            { name: 'Blocks', href: '/products/blocks' },
        ],
    },
    { name: 'Đơn hàng', href: '/orders', icon: ShoppingCart },
    { name: 'Thương hiệu', href: '/brands', icon: Tag },
    { name: 'Quảng cáo', href: '/advertising', icon: Megaphone },
    { name: 'Bình luận', href: '/comments', icon: MessageSquare },
    { name: 'Người dùng', href: '/users', icon: Users },
    { name: 'Cài đặt', href: '/settings', icon: Settings },
];

export function Sidebar() {
    const location = useLocation();
    const { logout, user } = useAuth();
    const [isMobileOpen, setIsMobileOpen] = useState(false);
    const [expandedMenus, setExpandedMenus] = useState<Set<string>>(new Set(['Bài viết', 'Sản phẩm']));

    const toggleMenu = (name: string) => {
        const newExpanded = new Set(expandedMenus);
        if (newExpanded.has(name)) {
            newExpanded.delete(name);
        } else {
            newExpanded.add(name);
        }
        setExpandedMenus(newExpanded);
    };

    const isActive = (href: string) => {
        if (href === '/') {
            return location.pathname === '/';
        }
        return location.pathname.startsWith(href);
    };

    const isParentActive = (children?: { href: string }[]) => {
        return children?.some((child) => isActive(child.href));
    };

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
                    'fixed inset-y-0 left-0 z-40 w-64 bg-card border-r transform transition-transform duration-200 ease-in-out lg:translate-x-0 flex flex-col',
                    isMobileOpen ? 'translate-x-0' : '-translate-x-full'
                )}
            >
                {/* Logo */}
                <div className="flex h-16 items-center px-6 border-b shrink-0">
                    <h1 className="text-xl font-bold text-primary">CMS Admin</h1>
                </div>

                {/* Navigation */}
                <nav className="flex-1 overflow-y-auto px-4 py-4 space-y-1">
                    {navigation.map((item) => {
                        if (item.children) {
                            const isExpanded = expandedMenus.has(item.name);
                            const parentActive = isParentActive(item.children);

                            return (
                                <div key={item.name}>
                                    <button
                                        onClick={() => toggleMenu(item.name)}
                                        className={cn(
                                            'flex items-center gap-3 w-full rounded-lg px-3 py-2 text-sm font-medium transition-colors',
                                            parentActive
                                                ? 'bg-primary/10 text-primary'
                                                : 'text-muted-foreground hover:bg-accent hover:text-accent-foreground'
                                        )}
                                    >
                                        <item.icon className="h-5 w-5" />
                                        <span className="flex-1 text-left">{item.name}</span>
                                        <ChevronDown
                                            className={cn(
                                                'h-4 w-4 transition-transform',
                                                isExpanded && 'rotate-180'
                                            )}
                                        />
                                    </button>
                                    {isExpanded && (
                                        <div className="ml-4 pl-4 border-l mt-1 space-y-1">
                                            {item.children.map((child) => (
                                                <Link
                                                    key={child.href}
                                                    to={child.href}
                                                    onClick={() => setIsMobileOpen(false)}
                                                    className={cn(
                                                        'flex items-center gap-3 rounded-lg px-3 py-2 text-sm transition-colors',
                                                        isActive(child.href)
                                                            ? 'bg-primary text-primary-foreground'
                                                            : 'text-muted-foreground hover:bg-accent hover:text-accent-foreground'
                                                    )}
                                                >
                                                    {child.name}
                                                </Link>
                                            ))}
                                        </div>
                                    )}
                                </div>
                            );
                        }

                        return (
                            <Link
                                key={item.name}
                                to={item.href!}
                                onClick={() => setIsMobileOpen(false)}
                                className={cn(
                                    'flex items-center gap-3 rounded-lg px-3 py-2 text-sm font-medium transition-colors',
                                    isActive(item.href!)
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
                <div className="border-t p-4 shrink-0">
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
            </aside>
        </>
    );
}
