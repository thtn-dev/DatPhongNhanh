import { Toaster } from '@/components/ui/sonner';
import { ThemeProvider } from '@/contexts/theme-context';

interface AppProviderProps {
  children: React.ReactNode;
}

function AppProvider({ children }: AppProviderProps) {
  return (
    <>
      <ThemeProvider defaultTheme='system'>
        {children}
        <Toaster />
      </ThemeProvider>
    </>
  );
}

export default AppProvider;
