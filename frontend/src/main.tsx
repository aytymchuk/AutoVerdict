import './app/instrumentation';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './app/styles/global.css';
import { App } from './app/App';

const container = document.getElementById('root');
if (!container) {
  throw new Error("Could not find root element with ID 'root' to mount the React application.");
}

createRoot(container).render(
  <StrictMode>
    <App />
  </StrictMode>,
);
