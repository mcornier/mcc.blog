import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './App.css';

const Comments = ({ articleId }) => {
  const [comments, setComments] = useState([]);
  const [content, setContent] = useState('');

  useEffect(() => {
    const fetchComments = async () => {
      try {
        const response = await axios.get(`/api/Comments/${articleId}`);
        setComments(response.data);
      } catch (error) {
        console.error('Error fetching comments:', error);
      }
    };

    fetchComments();
  }, [articleId]);

  const handleCommentSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post('/api/Comments', { content, articleId });
      setComments((prevComments) => [...prevComments, response.data]);
      setContent('');
    } catch (error) {
      console.error('Error adding comment:', error);
    }
  };

  return (
    <div className="comments-container">
      <h3>Comments</h3>
      <form onSubmit={handleCommentSubmit}>
        <textarea
          placeholder="Add a comment"
          value={content}
          onChange={(e) => setContent(e.target.value)}
          required
        />
        <button type="submit">Submit</button>
      </form>
      <ul>
        {comments.map(comment => (
          <li key={comment.id}>{comment.content}</li>
        ))}
      </ul>
    </div>
  );
};

export default Comments;
