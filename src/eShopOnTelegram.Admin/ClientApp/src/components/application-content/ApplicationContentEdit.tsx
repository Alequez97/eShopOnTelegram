import React, { useEffect, useState } from "react";
import axios from 'axios';
import {
  useDataProvider,
  useNotify,
  TextInput,
  SaveButton,
  useRefresh,
  SimpleForm,
} from "react-admin";

type LocalizationData = Record<string, string>;

const ApplicationContentEdit: React.FC = () => {
  const [localizationData, setLocalizationData] =
    useState<LocalizationData | null>(null);
  const dataProvider = useDataProvider();
  const notify = useNotify();
  const refresh = useRefresh();

  //   useEffect(() => {
  //     const fetchLocalizationData = async () => {
  //       try {
  //         const { data } = await dataProvider.getLocalization();
  //         setLocalizationData(data);
  //       } catch (error) {
  //         notify('Error fetching localization data', {type: 'error'});
  //       }
  //     };

  //     fetchLocalizationData();
  //   }, [dataProvider, notify]);

  useEffect(() => {
    const fetchLocalizationData = async () => {
      try {
        const { data } = await axios.get("/api/applicationContent"); // Adjust the endpoint URL according to your backend API
        setLocalizationData(data);
      } catch (error) {
        notify("Error fetching localization data", {type: "error"});
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
    <SimpleForm>
      {Object.entries(localizationData).map(([key, value]) => (
        <TextInput
          key={key}
          source={key}
          label={key}
          value={value}
          onChange={handleInputChange}
        />
      ))}
      <SaveButton handleSubmit={handleSave} />
    </SimpleForm>
  );
};

export default ApplicationContentEdit;
