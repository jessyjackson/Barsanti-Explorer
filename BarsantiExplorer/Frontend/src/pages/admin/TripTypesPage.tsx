import TripTypesForm from "@/components/TripTypesForm";
import { TripTypeFormData } from "@/components/TripTypesForm";
import { useToast } from "@/components/ui/use-toast";
import apiClient from "@/data/apiClient";
import { useNavigate } from "react-router-dom";
import TripTypeCard from "@/components/TripTypeCard";
import { useQuery } from "@tanstack/react-query";
import { AiOutlineLoading3Quarters } from "react-icons/ai";

function CreateTripTypesPage() {
    const navigate = useNavigate(); 
    const {toast}  = useToast();

    const tripTypesQuery = useQuery({
        queryKey: ["tripTypes"],
        queryFn: async () => {
            const tripTypes = await apiClient.tripTypesApi.apiTripTypesGet(
                undefined,
                "name",
            );
            return tripTypes.data;
        },
    });

    if(tripTypesQuery.isLoading){
        return(
        <div className="flex items-center justify-center mt-32">
            <AiOutlineLoading3Quarters className="animate-spin text-4xl mr-4" />
            <span>Loading...</span>
        </div>
        );
    };
    if (tripTypesQuery.isError) {
        return <div>Error: {tripTypesQuery.error.message}</div>;
    }

    const onSubmit = async (values: TripTypeFormData) => {
        try{
            console.log(values);
            const res = await apiClient.tripTypesApi.apiTripTypesPost(values.title);
            navigate(0)
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
            <div className="max-w-4xl  mx-auto"> 
                <h1 className="text-3xl font-bold">
                    Trip Type
                </h1>
                <TripTypesForm onSubmit={onSubmit}/>
            
                <h1 className="text-3xl font-bold mt-10">Existing types</h1>
                <div className="grid grid-cols-3 gap-10 mt-7">
                    {tripTypesQuery.data?.map((tripType) => (
                        <TripTypeCard title={tripType.name} key={`tripType-${tripType.id}`} />
                    ))}
                </div>
            </div>

        </main>
  );
}
export default CreateTripTypesPage;