import React, { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import EmptyLayout, {
  EmptyLayoutProps
} from '@/components/layouts/empty-layout';
import { withErrorHandler } from '@/error/error-handling';
import AppErrorFallback from '@/error/fallbacks/app-error-boundary-fallback';
import { FallbackProps } from 'react-error-boundary';

export const render = (Component: React.FC, rootId: string = 'root') => {
  createRoot(document.getElementById(rootId)!).render(
    <StrictMode>
      <Component />
    </StrictMode>
  );
};

export const renderWithFallback = (
  Component: React.FC,
  Layout: React.FC<EmptyLayoutProps> = EmptyLayout,
  rootId: string = 'root',
  FallbackComponent: React.FC<FallbackProps> = AppErrorFallback
) => {
  const ComponentWithErrorBoundary = withErrorHandler(
    Component,
    FallbackComponent
  );
  createRoot(document.getElementById(rootId)!).render(
    <StrictMode>
      <Layout>
        <ComponentWithErrorBoundary />
      </Layout>
    </StrictMode>
  );
};
