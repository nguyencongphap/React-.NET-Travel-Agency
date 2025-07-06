import { GoogleGenerativeAI } from "@google/generative-ai";
import { data, type ActionFunctionArgs } from "react-router";
import { AxiosPrivate } from "~/api/axios";
import { parseMarkdownToJson } from "~/lib/utils";

export const action = async ({ request }: ActionFunctionArgs) => {
  // Data we send from the form
  const {
    budget,
    country,
    groupType,
    interests,
    numberOfDays,
    travelStyle,
    userId,
  } = await request.json();

  // TODO: DEL LATER
  console.log("interests", interests);

  const genAI = new GoogleGenerativeAI(process.env.GEMINI_API_KEY!);
  const unsplashApiKey = process.env.UNSPLASH_ACCESS_KEY!;

  try {
    const prompt = `Generate a ${numberOfDays}-day travel itinerary for ${country} based on the following user information:
    Budget: '${budget}'
    Interests: '${interests}'
    TravelStyle: '${travelStyle}'
    GroupType: '${groupType}'
    Return the itinerary and lowest estimated price in a clean, non-markdown JSON format with the following structure 
    (Trip {
      name: string;
      description: string;
      estimatedPrice: string;
      duration: number;
      budget: string;
      travelStyle: string;
      country: string;
      interests: string;
      groupType: string;
      bestTimeToVisit: string[];
      weatherInfo: string[];
      location: Location;
      itinerary: [{ as described below }]
    })
    :
    {
    "name": "A descriptive title for the trip",
    "description": "A brief description of the trip and its highlights not exceeding 100 words",
    "estimatedPrice": "Lowest average price for the trip in USD, e.g.$price",
    "duration": ${numberOfDays},
    "budget": "${budget}",
    "travelStyle": "${travelStyle}",
    "country": "${country}",
    "interests": ${interests},
    "groupType": "${groupType}",
    "bestTimeToVisit": [
      '🌸 Season (from month to month): reason to visit',
      '☀️ Season (from month to month): reason to visit',
      '🍁 Season (from month to month): reason to visit',
      '❄️ Season (from month to month): reason to visit'
    ],
    "weatherInfo": [
      '☀️ Season: temperature range in Celsius (temperature range in Fahrenheit)',
      '🌦️ Season: temperature range in Celsius (temperature range in Fahrenheit)',
      '🌧️ Season: temperature range in Celsius (temperature range in Fahrenheit)',
      '❄️ Season: temperature range in Celsius (temperature range in Fahrenheit)'
    ],
    "location": {
      "city": "name of the city or region",
      "coordinates": [latitude, longitude],
      "openStreetMap": "link to open street map"
    },
    "itinerary": [
    {
      "day": 1,
      "location": "City/Region Name",
      "activities": [
        {"time": "Morning", "description": "🏰 Visit the local historic castle and enjoy a scenic walk"},
        {"time": "Afternoon", "description": "🖼️ Explore a famous art museum with a guided tour"},
        {"time": "Evening", "description": "🍷 Dine at a rooftop restaurant with local wine"}
      ]
    },
    ...
    ]
    }`;

    const textResult = await genAI
      .getGenerativeModel({ model: "gemini-2.5-pro" })
      .generateContent([prompt]);

    const trip = JSON.stringify(
      parseMarkdownToJson(textResult.response.text())
    );
    const imageResponse = await fetch(
      `https://api.unsplash.com/search/photos?query=${country} ${interests} ${travelStyle}&client_id=${unsplashApiKey}`
    );

    const imageUrls = (await imageResponse.json()).results
      .slice(0, 3)
      .map((res: any) => res.urls?.regular || null);

    // Save trip detail and image urls to db
    const resp = await AxiosPrivate.post<CreateTripResponse>(
      "Trip/CreateTrip",
      {
        TripDetail: trip,
        ImageUrls: imageUrls,
        UserId: userId,
      }
    );
    const result = resp.data;

    // this data() function is from reac router
    // that helps us returns data that is the result of this server action
    return data({ id: result.id });

    // put the image from Unsplash and the info from Gemini together
  } catch (error) {
    console.error("Error generating travel plan:", error);
  }
};

export type TripImageUrl = {
  id: number;
  value: string;
};

export type CreateTripResponse = {
  id: number;
  // tripDetail: string;
  // createdAt: Date;
  // tripImageUrls: TripImageUrl[];
};
