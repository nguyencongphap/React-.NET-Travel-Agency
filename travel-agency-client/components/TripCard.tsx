import { Link, useLocation } from "react-router";
import {
  ChipDirective,
  ChipListComponent,
  ChipsDirective,
} from "@syncfusion/ej2-react-buttons";
import { cn, getFirstWord } from "~/lib/utils";

type TripCardProps = {
  id: string;
  name: string;
  imageUrl: string;
  location: string;
  tags: string[];
  price: string;
};

const TripCard = ({
  id,
  name,
  imageUrl,
  location,
  tags,
  price,
}: TripCardProps) => {
  const path = useLocation();

  // for admins, we show trip details
  // for users, we show travel details
  const destination =
    path.pathname === "/" || path.pathname.startsWith("/travel")
      ? `/travel/${id}`
      : `/trips/${id}`;

  return (
    <Link to={destination} className="trip-card">
      <img src={imageUrl} alt={name} />

      <article>
        <h2>{name}</h2>
        {/* use figure instead of <p> for more usability with screen readers */}
        <figure>
          <img
            src="/assets/icons/location-mark.svg"
            alt="location"
            className="size-4"
          />
          <figcaption>{location}</figcaption>
        </figure>
      </article>

      <div className="mt-5 pl-[18px] pr-3.5 pb-5">
        <ChipListComponent id="travel-chip">
          <ChipsDirective>
            {tags &&
              tags.length > 0 &&
              tags.map((tag, index) => (
                <ChipDirective
                  key={index}
                  text={getFirstWord(tag)}
                  cssClass={cn(
                    index === 1
                      ? "!bg-pink-50 !text-pink-500"
                      : "!bg-success-50 !text-success-700"
                  )}
                />
              ))}
          </ChipsDirective>
        </ChipListComponent>
      </div>

      <article className="tripCard-pill">{price}</article>
    </Link>
  );
};

export default TripCard;
