import TripTypesForm from "@/components/TripTypesForm";
import { TripTypeFormData } from "@/components/TripTypesForm";
import { useToast } from "@/components/ui/use-toast";
import apiClient from "@/data/apiClient";
import { useNavigate } from "react-router-dom";


function CreateTripTypesPage() {
    const navigate = useNavigate();
    const {toast}  = useToast();
    const onSubmit = async (values: TripTypeFormData) => {
        try{
            console.log(values);
            const res = await apiClient.tripTypesApi.apiTripTypesPost(values.title);
            navigate(`/admin`);
            toast({
                title: "Trip Type created",
                description: "Your trip type has been created successfully",
                variant: "success",
            });
        }
        catch (error) {
            console.error(error);
            toast({
                title: "Failed to create trip",
                description: "An error occurred while creating the trip type",
                variant: "destructive",
            });
        }
    };
  return (
      <main className="page">
          <TripTypesForm onSubmit={onSubmit}/>
      </main>
  );
}
export default CreateTripTypesPage;