export const handleRedirect = (
    url: string,
    method: 'location' | 'form' = 'location'
  ) => {
    if (method === 'location') {
      window.location.href = url;
    } else {
      const form = document.createElement('form');
      form.method = 'GET';
      form.action = url;
      document.body.appendChild(form);
      form.submit();
    }
  };