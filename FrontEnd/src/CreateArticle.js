import React, { useState } from 'react';
import axios from 'axios';
import './App.css';

const CreateArticle = ({ onArticleCreated }) => {
  const [title, setTitle] = useState('');
  const [content, setContent] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post('/api/Articles', { title, content });
      onArticleCreated(response.data);
      setTitle('');
      setContent('');
      alert('Article created successfully');
    } catch (error) {
      console.error('Error creating article:', error);
      alert('Failed to create article');
    }
  };

  return (
    <div className="create-article-container">
      <h2>Create Article</h2>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          required
        />
        <textarea
          placeholder="Content"
          value={content}
          onChange={(e) => setContent(e.target.value)}
          required
        />
        <button type="submit">Create Article</button>
      </form>
    </div>
  );
};

export default CreateArticle;
