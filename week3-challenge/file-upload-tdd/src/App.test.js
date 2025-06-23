import { render, screen } from '@testing-library/react';
import App from './App';

test('renders File Upload Feature heading', () => {
  render(<App />);
  const heading = screen.getByText(/file upload feature/i);
  expect(heading).toBeInTheDocument();
});
