import {Card } from "./ui/card";
interface TripTypeCardProps {
    title: string;
}

function TripTypeCard(props: TripTypeCardProps) {
  return (
      <Card
          className="flex flex-col cursor-pointer overflow-hidden"
      >
            <div className="p-4">
                <h3 className="text-xl font-semibold text-auto">{props.title}</h3>
            </div>
      </Card>
      
  );
}
export default TripTypeCard;