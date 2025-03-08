import { ErrorInfo, FC } from 'react';
import { ErrorBoundary, FallbackProps } from 'react-error-boundary';

function getDisplayName(WrappedComponent: FC) {
  return WrappedComponent.displayName || WrappedComponent.name || 'Component';
}

function withErrorHandler<P extends object>(
  Component: FC<P>,
  Fallback: FC<FallbackProps>,
  onError?: (error: Error, info: ErrorInfo) => void,
  onReset?: () => void
) {
  function ComponentWithErrorHandling(props: P) {
    return (
      <ErrorBoundary FallbackComponent={Fallback} onError={onError} onReset={onReset}>
        <Component {...(props as P)} />
      </ErrorBoundary>
    );
  }

  ComponentWithErrorHandling.displayName = `WithErrorHandling${getDisplayName(
    Component as FC<unknown>
  )}`;

  return ComponentWithErrorHandling;
}

export { withErrorHandler };
