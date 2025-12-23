"use client";

import { Suspense, useState } from "react";
import Link from "next/link";
import { useRouter, useSearchParams } from "next/navigation";
import { Eye, EyeOff, Loader2 } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useAuth } from "@/hooks/use-auth";
import { authApi } from "@/lib/api";

function LoginForm() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const { login } = useAuth();
  
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  const redirectUrl = searchParams.get("redirect") || "/";

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    setError("");
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError("");

    try {
      const response = await authApi.login(formData);
      login(response.data.user, response.data.token);
      router.push(redirectUrl);
    } catch {
      setError("Email hoặc mật khẩu không chính xác");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="bg-white rounded-lg border p-6 shadow-sm">
      <form onSubmit={handleSubmit} className="space-y-4">
        {error && (
          <div className="bg-red-50 text-red-600 text-sm p-3 rounded-md">
            {error}
          </div>
        )}

        <div className="space-y-2">
          <label className="text-sm font-medium">Email</label>
          <Input
            name="email"
            type="email"
            value={formData.email}
            onChange={handleChange}
            placeholder="your@email.com"
            required
          />
        </div>

        <div className="space-y-2">
          <label className="text-sm font-medium">Mật khẩu</label>
          <div className="relative">
            <Input
              name="password"
              type={showPassword ? "text" : "password"}
              value={formData.password}
              onChange={handleChange}
              placeholder="••••••••"
              required
              className="pr-10"
            />
            <button
              type="button"
              onClick={() => setShowPassword(!showPassword)}
              className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-600"
            >
              {showPassword ? (
                <EyeOff className="h-4 w-4" />
              ) : (
                <Eye className="h-4 w-4" />
              )}
            </button>
          </div>
        </div>

        <div className="flex items-center justify-between text-sm">
          <label className="flex items-center gap-2">
            <input type="checkbox" className="rounded" />
            <span className="text-gray-600">Ghi nhớ đăng nhập</span>
          </label>
          <Link href="/quen-mat-khau" className="text-primary hover:underline">
            Quên mật khẩu?
          </Link>
        </div>

        <Button type="submit" className="w-full" disabled={isLoading}>
          {isLoading ? (
            <>
              <Loader2 className="mr-2 h-4 w-4 animate-spin" />
              Đang xử lý...
            </>
          ) : (
            "Đăng nhập"
          )}
        </Button>
      </form>

      <div className="mt-6 text-center text-sm">
        <span className="text-gray-500">Chưa có tài khoản? </span>
        <Link href="/dang-ky" className="text-primary font-medium hover:underline">
          Đăng ký ngay
        </Link>
      </div>
    </div>
  );
}

export default function LoginPage() {
  return (
    <div className="min-h-[80vh] flex items-center justify-center py-12 px-4">
      <div className="w-full max-w-md">
        <div className="text-center mb-8">
          <h1 className="text-3xl font-bold text-gray-900">Đăng nhập</h1>
          <p className="text-gray-500 mt-2">
            Chào mừng bạn quay trở lại!
          </p>
        </div>

        <Suspense fallback={<div className="text-center p-6">Đang tải...</div>}>
          <LoginForm />
        </Suspense>
      </div>
    </div>
  );
}
