import AppProvider from '../providers/app-provider';

export interface EmptyLayoutProps {
  children: React.ReactNode;
}

function EmptyLayout(props: EmptyLayoutProps) {
  return <AppProvider>{props.children}</AppProvider>;
}

export default EmptyLayout;
