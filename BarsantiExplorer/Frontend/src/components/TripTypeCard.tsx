import {Card } from "./ui/card";
import { GoTrash } from "react-icons/go";
import { Button } from "./ui/button";
import { useState } from "react";
import {
  AlertDialog,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from "@/components/ui/alert-dialog";
import { useNavigate, useParams } from "react-router-dom";
import apiClient from "@/data/apiClient";
import {useMutation } from "@tanstack/react-query";
import { useToast } from "@/components/ui/use-toast";
import { TripType } from "@/apiClient";

interface TripTypeCardProps {
  type: TripType;
}

function TripTypeCard(props: TripTypeCardProps) {
  const { id } = useParams();
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const navigate = useNavigate();
  const { toast } = useToast();
  const deleteTypeMutation = useMutation({
    mutationFn: async (typeId: number) => {
      await apiClient.tripTypesApi.apiTripTypesIdDelete(typeId);
    },
    onSuccess: () => {
      setDeleteDialogOpen(false);
      toast({
        title: "Type deleted",
        description: "The type has been deleted successfully",
        variant: "success",
      });
      navigate(0);
    },
    onError: (error) => {
      toast({
        title: "Error",
        description: error.message,
        variant: "destructive",
      });
    },
  });
  const buildDeleteTypeDialog = () => {
    return (
      <AlertDialog open={deleteDialogOpen} onOpenChange={setDeleteDialogOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Delete type</AlertDialogTitle>
          </AlertDialogHeader>
          <AlertDialogDescription>
            Are you sure you want to delete this type?
          </AlertDialogDescription>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <Button
              variant="destructive"
              loading={deleteTypeMutation.isPending}
              onClick={() => {
                deleteTypeMutation.mutate(props.type.id);
              }}
            >
              Delete
            </Button>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    );
  };
  return (
      <div>
          
      <Card
          className="flex flex-col cursor-pointer overflow-hidden"
      >
          <div className="p-4 relative flex">
            <h3 className="text-xl font-semibold">{props.type.name}</h3>
            <Button 
            variant="ghost"
            className="text-4xl absolute right-5"
            onClick={() => setDeleteDialogOpen(true)}>
                  <GoTrash color="red"/>
            </Button>
        </div>
      </Card>
      {buildDeleteTypeDialog()}
      </div>
  );
}
export default TripTypeCard;