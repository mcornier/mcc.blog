import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './App.css';
import Comments from './Comments';

const Articles = () => {
  const [articles, setArticles] = useState([]);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(false);
  const [selectedArticle, setSelectedArticle] = useState(null);

  useEffect(() => {
    const fetchArticles = async () => {
      setLoading(true);
      try {
        const response = await axios.get(`/api/Articles?page=${page}`);
        setArticles((prevArticles) => [...prevArticles, ...response.data]);
      } catch (error) {
        console.error('Error fetching articles:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchArticles();
  }, [page]);

  const handleScroll = () => {
    if (window.innerHeight + window.scrollY >= document.body.offsetHeight - 500 && !loading) {
      setPage((prevPage) => prevPage + 1);
    }
  };

  useEffect(() => {
    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, [loading]);

  const handleArticleClick = (article) => {
    setSelectedArticle(article);
  };

  return (
    <div className="container">
      <h2>Articles</h2>
      {articles.map(article => (
        <div key={article.id} className="article" onClick={() => handleArticleClick(article)}>
          <h3>{article.title}</h3>
          <p>{article.content.substring(0, 100)}...</p>
          <small>By {article.author} on {new Date(article.createdDate).toLocaleDateString()}</small>
        </div>
      ))}
      {loading && <p>Loading more articles...</p>}
      {selectedArticle && (
        <div className="full-article">
          <h2>{selectedArticle.title}</h2>
          <p>{selectedArticle.content}</p>
          <Comments articleId={selectedArticle.id} />
          <button onClick={() => setSelectedArticle(null)}>Close</button>
        </div>
      )}
    </div>
  );
};

export default Articles;
