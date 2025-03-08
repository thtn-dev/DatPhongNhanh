import { FallbackProps } from 'react-error-boundary';

function AppErrorFallback({ error, resetErrorBoundary }: FallbackProps) {
  return (
    <div
      role='alert'
      style={{
        padding: '20px',
        margin: '20px',
        border: '1px solid red',
        borderRadius: '5px',
        backgroundColor: '#fff0f0'
      }}
    >
      <h2>Đã xảy ra lỗi:</h2>
      <p style={{ color: 'red' }}>{error.message}</p>
      <button
        onClick={resetErrorBoundary}
        style={{
          padding: '8px 16px',
          backgroundColor: '#4CAF50',
          color: 'white',
          border: 'none',
          borderRadius: '4px',
          cursor: 'pointer'
        }}
      >
        Thử lại
      </button>
    </div>
  );
}

export default AppErrorFallback;
