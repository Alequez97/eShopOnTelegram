import { MouseEventHandler } from "react";
import styled from "styled-components";

interface ButtonProps {
  type: "add" | "remove";
  title: string;
  disabled: boolean;
  onClick: MouseEventHandler<HTMLElement>;
}

const buttonStyles: Record<ButtonProps["type"], any> = {
  add: {
    normal: "rgb(75, 226, 75)",
    hover: "#ad9a1c",
    active: "#0b8325",
  },
  remove: {
    normal: "tomato",
    hover: "rgb(209, 83, 61)",
    active: "rgb(189, 83, 61)",
  },
};

const StyledButton = styled.button<ButtonProps>`
  color: black;
  padding: 0.6rem 0.8rem;
  font-size: 1.2rem;
  text-align: center;
  border: 0;
  outline: none;
  border-radius: 10px;
  width: 120px;
  margin-left: 10px;
  box-shadow: 1px -3px 12px -5px var(--text-color);
  background-color: ${(props) => buttonStyles[props.type].normal};

  &:hover {
    background-color: ${(props) => buttonStyles[props.type].hover};
  }

  &:active {
    background-color: ${(props) => buttonStyles[props.type].active};
    box-shadow: none;
  }

  &:disabled {
    background-color: #ccc;
    color: #666;
    cursor: not-allowed;
  }
`;

export const Button = (props: ButtonProps) => {
  return <StyledButton {...props}>{props.title}</StyledButton>;
};
