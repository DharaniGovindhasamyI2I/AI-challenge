import React, { useState } from 'react';
import './FileUploader.css';

function FileUploader({ simulateError = false }) {
  const [file, setFile] = useState(null);
  const [message, setMessage] = useState('');
  const [status, setStatus] = useState(''); // '', 'success', 'error'

  const handleChange = (e) => {
    setFile(e.target.files[0]);
    setMessage('');
    setStatus('');
  };

  const handleUpload = async () => {
    if (!file) {
      setMessage('Please select a file');
      setStatus('error');
      return;
    }
    // Simulate upload
    setStatus('');
    setMessage('Uploading...');
    await new Promise((resolve) => setTimeout(resolve, 600));
    if (simulateError) {
      setMessage('Upload failed');
      setStatus('error');
    } else {
      setMessage('File uploaded successfully');
      setStatus('success');
    }
  };

  return (
    <div className="file-uploader-container">
      <label className="file-label">
        <span className="file-label-text">Choose file</span>
        <input type="file" aria-label="choose file" onChange={handleChange} className="file-input" />
      </label>
      {file && <div className="file-name">Selected: {file.name}</div>}
      <button className="upload-btn" onClick={handleUpload}>Upload</button>
      {message && (
        <div className={`upload-message ${status}`}>{message}</div>
      )}
    </div>
  );
}

export default FileUploader; 