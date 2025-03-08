import type React from 'react';
import { useEffect, useRef, useState } from 'react';
import { useMutation } from '@tanstack/react-query';
import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { handleRedirect } from '@/utils/redirect';
import { Loader2 } from 'lucide-react';
import { toast } from 'sonner';

// Mock API function to verify 2FA code
const verifyTwoFactorCode = async (code: string) => {
  // In a real app, this would be an API call
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (code === '123456') {
        resolve({ success: true });
      } else {
        reject(new Error('Invalid code'));
      }
    }, 1000);
  });
};

export default function TwoFactorForm() {
  const [code, setCode] = useState(['', '', '', '', '', '']);
  const inputRefs = useRef<(HTMLInputElement | null)[]>([]);

  // Focus the first input on mount
  useEffect(() => {
    if (inputRefs.current[0]) {
      inputRefs.current[0].focus();
    }
  }, []);

  const mutation = useMutation({
    mutationFn: (code: string) => verifyTwoFactorCode(code),
    onSuccess: () => {
      toast('Authentication successful');

      setTimeout(() => {
        handleRedirect('/');
      }, 1000);
    },
    onError: () => {
      toast('Invalid code. Please try again.');

      // Reset the code
      setCode(['', '', '', '', '', '']);
      if (inputRefs.current[0]) {
        inputRefs.current[0].focus();
      }
    }
  });

  const handleInputChange = (index: number, value: string) => {
    // Only allow numbers
    if (value && !/^\d+$/.test(value)) return;

    const newCode = [...code];

    // Handle paste event (multiple characters)
    if (value.length > 1) {
      const pastedChars = value.split('').slice(0, 6);

      for (let i = 0; i < pastedChars.length; i++) {
        if (index + i < 6) {
          newCode[index + i] = pastedChars[i];
        }
      }

      setCode(newCode);

      // Focus the next empty input or the last one
      const nextIndex = Math.min(index + pastedChars.length, 5);
      if (inputRefs.current[nextIndex]) {
        inputRefs.current[nextIndex].focus();
      }

      return;
    }

    // Handle single character input
    newCode[index] = value;
    setCode(newCode);

    // Auto-focus next input if current input is filled
    if (value && index < 5) {
      if (inputRefs.current[index + 1]) {
        inputRefs.current[index + 1]?.focus();
      }
    }

    // Submit automatically when all fields are filled
    if (newCode.every((digit) => digit) && !newCode.includes('')) {
      handleSubmit();
    }
  };

  const handleKeyDown = (
    index: number,
    e: React.KeyboardEvent<HTMLInputElement>
  ) => {
    // Move to previous input on backspace if current input is empty
    if (e.key === 'Backspace' && !code[index] && index > 0) {
      if (inputRefs.current[index - 1]) {
        inputRefs.current[index - 1]?.focus();
      }
    }
  };

  const handleSubmit = () => {
    const fullCode = code.join('');
    if (fullCode.length === 6) {
      mutation.mutate(fullCode);
    }
  };

  return (
    <Card>
      <CardContent className='pt-6'>
        <div className='space-y-4'>
          <div className='space-y-2'>
            <Label htmlFor='code-1'>Verification Code</Label>
            <div className='flex justify-between gap-2'>
              {code.map((digit, index) => (
                <Input
                  key={index}
                  id={`code-${index + 1}`}
                  type='text'
                  inputMode='numeric'
                  maxLength={6}
                  value={digit}
                  onChange={(e) => handleInputChange(index, e.target.value)}
                  onKeyDown={(e) => handleKeyDown(index, e)}
                  ref={(el) => {
                    inputRefs.current[index] = el;
                  }}
                  className='h-12 w-12 text-center text-lg'
                />
              ))}
            </div>
            <p className='text-xs text-muted-foreground'>
              Enter the 6-digit code sent to your device
            </p>
          </div>

          <Button
            type='button'
            className='w-full'
            onClick={handleSubmit}
            disabled={code.includes('') || mutation.isLoading}
          >
            {mutation.isLoading ? (
              <>
                <Loader2 className='mr-2 h-4 w-4 animate-spin' />
                Verifying...
              </>
            ) : (
              'Verify'
            )}
          </Button>

          <div className='text-center'>
            <Button variant='link' className='text-xs text-muted-foreground'>
              Didn't receive a code? Resend
            </Button>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
