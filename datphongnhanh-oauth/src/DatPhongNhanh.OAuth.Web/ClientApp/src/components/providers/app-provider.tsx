import { Toaster } from '@/components/ui/sonner';
import { ThemeProvider } from '@/contexts/theme-context';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

interface AppProviderProps {
  children: React.ReactNode;
}

const queryClient = new QueryClient();

function AppProvider({ children }: AppProviderProps) {
  return (
    <>
      <ThemeProvider defaultTheme='system'>
        <QueryClientProvider client={queryClient}>
        {children}
        </QueryClientProvider>
        <Toaster />
      </ThemeProvider>
    </>
  );
}

export default AppProvider;
