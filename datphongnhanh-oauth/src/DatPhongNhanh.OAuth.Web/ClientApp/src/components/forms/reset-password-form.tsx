'use client';

import * as z from 'zod';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useMutation } from '@tanstack/react-query';
import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { handleRedirect } from '@/utils/redirect';
import { Loader2 } from 'lucide-react';
import { toast } from 'sonner';

const formSchema = z
  .object({
    password: z.string().min(8, {
      message: 'Password must be at least 8 characters.'
    }),
    confirmPassword: z.string()
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: 'Passwords do not match',
    path: ['confirmPassword']
  });

type FormValues = z.infer<typeof formSchema>;

// Mock API function to reset password
const resetPassword = async (data: FormValues & { token: string }) => {
  // In a real app, this would be an API call that includes the token
  return new Promise((resolve) => {
    setTimeout(() => {
      console.log(data);
      resolve({ success: true });
    }, 1000);
  });
};

export default function ResetPasswordForm() {
  const token = '';

  const form = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      password: '',
      confirmPassword: ''
    }
  });

  const mutation = useMutation({
    mutationFn: (data: FormValues) => resetPassword({ ...data, token }),
    onSuccess: () => {
      toast('Password reset successful', {
        description:
          'Your password has been reset. You can now log in with your new password.'
      });
      setTimeout(() => {
        handleRedirect('/login');
      }, 2000);
    },
    onError: () => {
      toast('Password reset failed', {
        description:
          'There was a problem resetting your password. The link may have expired.'
      });
    }
  });

  function onSubmit(data: FormValues) {
    if (!token) {
      toast('Invalid reset link', {
        description: 'This password reset link is invalid or has expired.'
      });
      return;
    }

    mutation.mutate(data);
  }

  return (
    <Card>
      <CardContent className='pt-6'>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className='space-y-4'>
            <FormField
              control={form.control}
              name='password'
              render={({ field }) => (
                <FormItem>
                  <FormLabel>New Password</FormLabel>
                  <FormControl>
                    <Input
                      type='password'
                      placeholder='Create a new password'
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name='confirmPassword'
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Confirm Password</FormLabel>
                  <FormControl>
                    <Input
                      type='password'
                      placeholder='Confirm your new password'
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <Button
              type='submit'
              className='w-full'
              disabled={mutation.isLoading}
            >
              {mutation.isLoading ? (
                <>
                  <Loader2 className='mr-2 h-4 w-4 animate-spin' />
                  Resetting password...
                </>
              ) : (
                'Reset password'
              )}
            </Button>
          </form>
        </Form>
      </CardContent>
    </Card>
  );
}
