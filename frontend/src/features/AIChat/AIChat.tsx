import { useAuth } from '@clerk/clerk-react';
import { useState, useEffect } from 'react';

export function AIChat() {
  const { getToken } = useAuth();
  const [token, setToken] = useState<string | null>(null);

  useEffect(() => {
    getToken().then(setToken);
  }, [getToken]);

  return (
    <div className="flex flex-col h-[600px] border border-gray-200 rounded-lg overflow-hidden bg-white shadow-sm">
      <div className="p-4 bg-yellow-50 border-b border-yellow-200 text-xs break-all text-yellow-800">
        <span className="font-semibold block mb-1">Auth Token:</span>
        {token || 'Loading...'}
      </div>
      <div className="flex-1 overflow-y-auto p-4 space-y-4">
        {/* Chat UI will be implemented here */}
      </div>

      <form
        onSubmit={(e) => e.preventDefault()}
        className="p-4 border-t border-gray-200 bg-gray-50 flex gap-2"
      >
        <input
          placeholder="Ask AutoVerdikt..."
          className="flex-1 p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
        <button
          type="submit"
          className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition-colors"
        >
          Send
        </button>
      </form>
    </div>
  );
}
