import React from "react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";

const formSchema = z.object({
    title: z.string(),
});

export type TripTypeFormData = z.infer<typeof formSchema>;

interface TripTypeFormProps {
    onSubmit: (values: TripTypeFormData) => void;
    defaultValues?: TripTypeFormData;
}

function TripTypesForm(props: TripTypeFormProps) {
    const form = useForm<TripTypeFormData>({
        resolver: zodResolver(formSchema),
        defaultValues: props.defaultValues,
    });


    return (
        <Form {...form}>
            <form
                onSubmit={form.handleSubmit(props.onSubmit)}
                className="max-w-2xl w-full mx-auto"
            >
                <h1 className="text-3xl font-bold">
                    Create Trip Type
                </h1>
                <FormField
                    control={form.control}
                    name="title"
                    render={({ field }) => (
                        <FormItem className="w-full mt-6">
                            <FormLabel>Title</FormLabel>
                            <FormControl>
                                <Input placeholder="Enter a title" {...field} />
                            </FormControl>
                            <FormMessage />
                        </FormItem> 
                    )}
                />
                <Button
                    type="submit"
                    className="w-full mt-6"
                    loading={form.formState.isSubmitting}
                >
                    Create Trip Type
                </Button>
            </form>
        </Form>
    );
}
export default TripTypesForm;