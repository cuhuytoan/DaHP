import Image from "next/image";
import Link from "next/link";
import { Card, CardContent } from "@/components/ui/card";
import { formatDate, truncateText } from "@/lib/utils";
import { Article } from "@/types";

interface ArticleCardProps {
  article: Article;
}

export function ArticleCard({ article }: ArticleCardProps) {
  return (
    <Card className="group overflow-hidden hover:shadow-lg transition-shadow duration-300">
      <Link href={`/tin-tuc/${article.id}`}>
        <div className="relative aspect-video overflow-hidden bg-gray-100">
          {article.image ? (
            <Image
              src={article.image}
              alt={article.name}
              fill
              className="object-cover group-hover:scale-105 transition-transform duration-300"
              sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
            />
          ) : (
            <div className="w-full h-full flex items-center justify-center text-gray-400">
              <span className="text-4xl">📰</span>
            </div>
          )}
        </div>

        <CardContent className="p-4">
          <p className="text-xs text-gray-500 mb-2">
            {article.articleStatusName || article.author} • {article.createDate && formatDate(article.createDate)}
          </p>
          <h3 className="font-medium text-gray-900 line-clamp-2 group-hover:text-primary transition-colors mb-2">
            {article.name}
          </h3>
          {article.description && (
            <p className="text-sm text-gray-600 line-clamp-2">
              {truncateText(article.description, 120)}
            </p>
          )}
        </CardContent>
      </Link>
    </Card>
  );
}
