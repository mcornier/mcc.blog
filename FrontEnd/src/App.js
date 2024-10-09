import React, { useState } from 'react';
import Articles from './Articles';
import Auth from './Auth';
import CreateArticle from './CreateArticle';
import './App.css';

function App() {
  const [articles, setArticles] = useState([]);

  const handleArticleCreated = (newArticle) => {
    setArticles((prevArticles) => [newArticle, ...prevArticles]);
  };

  return (
    <div>
      <header>
        <h1>Blog Frontend</h1>
      </header>
      <Auth />
      <CreateArticle onArticleCreated={handleArticleCreated} />
      <Articles articles={articles} />
    </div>
  );
}

export default App;
