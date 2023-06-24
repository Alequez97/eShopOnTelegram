import React, { useEffect, useRef, useState } from "react";
import axios from "axios";
import { useNotify, TextInput, SaveButton, SimpleForm } from "react-admin";

type LocalizationData = Record<string, string>;

const ApplicationContentEdit: React.FC = () => {
  const [localizationData, setLocalizationData] =
    useState<LocalizationData | null>(null);

  const notify = useNotify();

  useEffect(() => {
    const fetchLocalizationData = async () => {
      try {
        const { data } = await axios.get("/api/applicationContent"); // Adjust the endpoint URL according to your backend API
        setLocalizationData(data);
      } catch (error) {
        notify("Error fetching localization data", { type: "error" });
      }
    };

    fetchLocalizationData();
  }, [notify]);

  const handleSave = async () => {
    console.log("save");
    // try {
    //   if (localizationData) {
    //     await dataProvider.updateLocalization(localizationData);
    //     notify('Localization data saved');
    //     refresh(); // Refresh the data on the page to reflect the changes
    //   }
    // } catch (error) {
    //   notify('Error saving localization data', {type: 'error'});
    // }
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setLocalizationData((prevData) => ({
      ...(prevData as LocalizationData),
      [name]: value,
    }));
  };

  if (!localizationData) {
    return <div>Loading...</div>;
  }

  return (
    <SimpleForm onSubmit={() => console.log("on submit callback")}>
      {Object.entries(localizationData).map(([key, value], index, array) => {
        if (
          index === 0 ||
          key.split(".")[0] !== array[index - 1][0].split(".")[0]
        ) {
          const groupName = key.split(".")[0];

          return (
            <React.Fragment key={groupName}>
              <h2>{groupName}</h2> {/* Render the group name */}
              <TextInput
                key={key}
                source={key}
                label={key
                  .split(".")[1]
                  .split(/(?=[A-Z])/)
                  .map((word, index) =>
                    index === 0 ? word : word.toLowerCase()
                  )
                  .join(" ")}
                defaultValue={value}
                onChange={handleInputChange}
                fullWidth
              />
            </React.Fragment>
          );
        }

        return (
          <TextInput
            key={key}
            source={key}
            label={key
              .split(".")[1]
              .split(/(?=[A-Z])/)
              .map((word, index) => (index === 0 ? word : word.toLowerCase()))
              .join(" ")}
            defaultValue={value}
            onChange={handleInputChange}
            fullWidth
          />
        );
      })}
    </SimpleForm>
  );
};

export default ApplicationContentEdit;
