import { AIChat } from '../features/AIChat/AIChat';
import { SignedIn, SignedOut, SignInButton } from '@clerk/clerk-react';

export function ChatPage() {
  return (
    <div className="max-w-4xl mx-auto space-y-6">
      <div className="text-center">
        <h2 className="text-2xl font-bold text-gray-800">Vehicle Investigation Assistant</h2>
        <p className="text-gray-600 mt-2">
          Ask questions about vehicle history, CEPiK records, or upload documents for analysis.
        </p>
      </div>

      <SignedIn>
        <AIChat />
      </SignedIn>

      <SignedOut>
        <div className="bg-white p-8 rounded-lg shadow-sm text-center border border-gray-200">
          <h3 className="text-lg font-medium text-gray-900 mb-4">Please sign in to continue</h3>
          <SignInButton mode="modal">
            <button className="px-6 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition-colors">
              Sign In
            </button>
          </SignInButton>
        </div>
      </SignedOut>
    </div>
  );
}
