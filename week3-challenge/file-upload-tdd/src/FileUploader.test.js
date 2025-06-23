import React from 'react';
import { render, fireEvent, screen, waitFor } from '@testing-library/react';
import FileUploader from './FileUploader';

describe('FileUploader', () => {
  test('renders file input and upload button', () => {
    render(<FileUploader />);
    expect(screen.getByLabelText(/choose file/i)).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /upload/i })).toBeInTheDocument();
  });

  test('shows error if no file is selected and upload is clicked', async () => {
    render(<FileUploader />);
    fireEvent.click(screen.getByRole('button', { name: /upload/i }));
    expect(await screen.findByText(/please select a file/i)).toBeInTheDocument();
  });

  test('shows success message after file upload', async () => {
    render(<FileUploader />);
    const file = new File(['hello'], 'hello.txt', { type: 'text/plain' });
    fireEvent.change(screen.getByLabelText(/choose file/i), { target: { files: [file] } });
    fireEvent.click(screen.getByRole('button', { name: /upload/i }));
    expect(await screen.findByText(/file uploaded successfully/i)).toBeInTheDocument();
  });

  test('shows error message if upload fails', async () => {
    render(<FileUploader simulateError />);
    const file = new File(['fail'], 'fail.txt', { type: 'text/plain' });
    fireEvent.change(screen.getByLabelText(/choose file/i), { target: { files: [file] } });
    fireEvent.click(screen.getByRole('button', { name: /upload/i }));
    expect(await screen.findByText(/upload failed/i)).toBeInTheDocument();
  });
}); 