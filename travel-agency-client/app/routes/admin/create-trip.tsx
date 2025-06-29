import { Header } from "components";
import { ComboBoxComponent } from "@syncfusion/ej2-react-dropdowns";
import type { Route } from "./+types/create-trip";
import type { Country, TripFormData } from "~/index";

export const loader = async () => {
  const response = await fetch(
    "https://restcountries.com/v3.1/all?fields=flag,name,latlng,maps"
  );
  const data = await response.json();

  return data.map((country: any) => ({
    name: country.flag + " " + country.name.common,
    coordinates: country.latlng,
    value: country.name.common,
    openStreetMap: country.maps?.openStreetMaps,
  }));
};

const CreateTrip = ({ loaderData }: Route.ComponentProps) => {
  const handleSubmit = async () => {};
  const handleChange = (key: keyof TripFormData, value: string | number) => {};

  const countries = loaderData as Country[];

  const countryData = countries.map((country) => ({
    text: country.name,
    value: country.value,
  }));

  // TODO: DEL LATER
  console.log("countryData", countryData);

  return (
    <main className="flex flex-col gap-10 pb-20 wrapper">
      <Header
        title="Add a New Trip"
        description="View and edit AI Generated travel plans"
      />

      <section>
        <form className="trip-form" onSubmit={handleSubmit}>
          <div>
            <label htmlFor="country">Country</label>
            <ComboBoxComponent
              id="country"
              dataSource={countryData}
              fields={{ text: "text", value: "value" }}
              placeholder="Select a Country"
              className="combo-box"
              change={(e: { value: string | undefined }) => {
                if (e.value) {
                  handleChange("country", e.value);
                }
              }}
              // turn this on since we use the "change" prop
              allowFiltering
              // define which property of the data to use to filter the data by
              filtering={(e) => {
                const query = e.text.toLowerCase();

                e.updateData(
                  countries
                    .filter((country) =>
                      country.name.toLowerCase().includes(query)
                    )
                    .map((country) => ({
                      text: country.name,
                      value: country.value,
                    }))
                );
              }}
            />
          </div>
        </form>
      </section>
    </main>
  );
};

export default CreateTrip;
